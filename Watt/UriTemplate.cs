using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Cloud.WebApi.TemplateToolkit
{
	using Templates;

	/// <summary>
	///		Populates parameterised URI templates.
	/// </summary>
	public sealed class UriTemplate
	{
		/// <summary>
		///		The URI template.
		/// </summary>
		readonly string							_template;

		/// <summary>
		///		The template's URI segments.
		/// </summary>
		readonly IReadOnlyList<UriSegment>		_uriSegments;

		/// <summary>
		///		The template's URI segments.
		/// </summary>
		readonly IReadOnlyList<QuerySegment>	_querySegments;

		/// <summary>
		///		Create a new URI template.
		/// </summary>
		/// <param name="template">
		///		The template.
		/// </param>
		public UriTemplate(string template)
		{
			if (String.IsNullOrWhiteSpace(template))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'template'.", "template");

			_template = template;

			IReadOnlyList<TemplateSegment> templateSegments = TemplateSegment.Parse(_template);
			_uriSegments = templateSegments.OfType<UriSegment>().ToArray();
			if (_uriSegments.Count == 0)
				throw new UriTemplateException("Invalid URI template (contains no path segments).");

			_querySegments = templateSegments.OfType<QuerySegment>().ToArray();
		}

		/// <summary>
		///		Build a URI from the template.
		/// </summary>
		/// <param name="baseUri">
		///		The base URI.
		/// </param>
		/// <param name="templateParameters">
		///		A dictionary containing the template parameters.
		/// </param>
		/// <returns>
		///		The generated URI.
		/// </returns>
		public Uri Populate(Uri baseUri, IDictionary<string, string> templateParameters)
		{
			if (baseUri == null)
				throw new ArgumentNullException("baseUri");

			if (!baseUri.IsAbsoluteUri)
				throw new UriTemplateException("URI '{0}' is not an absolute URI.", baseUri);

			if (templateParameters == null)
				throw new ArgumentNullException("templateParameters");

			TemplateEvaluationContext evaluationContext = new TemplateEvaluationContext(templateParameters);
			StringBuilder uriBuilder = new StringBuilder(
				baseUri.GetComponents(UriComponents.Scheme | UriComponents.StrongAuthority, UriFormat.UriEscaped)
			);
			if (_uriSegments.Count > 0)
			{
				foreach (UriSegment uriSegment in _uriSegments)
				{
					string segmentValue = uriSegment.GetValue(evaluationContext);
					if (segmentValue == null)
						continue;

					uriBuilder.Append(
						Uri.EscapeUriString(segmentValue)
					);
					if (uriSegment.IsDirectory)
						uriBuilder.Append('/');
				}
			}
			else
				uriBuilder.Append('/');

			if (_querySegments.Count > 0)
			{
				Action<QuerySegment, char> appendSegment = (segment, separator) =>
				{
					string queryParameterValue = segment.GetValue(evaluationContext);
					if (queryParameterValue == null)
						return;

					string queryParameterName = segment.QueryParameterName;

					uriBuilder.Append(separator);
					uriBuilder.AppendFormat(
						"{0}={1}",
						queryParameterName,
						Uri.EscapeDataString(queryParameterValue)
					);
				};

				appendSegment(_querySegments[0], '?');
				for (int segmentIndex = 1; segmentIndex < _querySegments.Count; segmentIndex++)
					appendSegment(_querySegments[segmentIndex], '&');
			}

			return new Uri(uriBuilder.ToString());
		}
	}
}
