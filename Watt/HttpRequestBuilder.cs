using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Formatting;

namespace DD.Cloud.WebApi.TemplateToolkit
{
	using System.Collections.Generic;
	using Utilities;

	/// <summary>
	///		Builds and submits HTTP requests for an <see cref="System.Net.Http.HttpClient"/>.
	/// </summary>
	public static class HttpRequestBuilder
	{
		#region Constants

		/// <summary>
		///		No media-type formatters.
		/// </summary>
		internal static readonly ImmutableHashSet<MediaTypeFormatter>		NoMediaTypeFormatters = ImmutableHashSet.Create(MediaTypeFormatterEqualityComparer.ByType);

		#endregion // Constants

		/// <summary>
		///		Create a new HTTP request builder that is not attached to an <see cref="System.Net.Http.HttpClient"/>.
		/// </summary>
		/// <param name="requestUri">
		///		The request URI (can be relative or absolute).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<Unit> Create(string requestUri)
		{
			if (String.IsNullOrWhiteSpace(requestUri))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'requestUri'.", "requestUri");

			return Create(
				requestUri: new Uri(requestUri, UriKind.RelativeOrAbsolute)
			);
		}

		/// <summary>
		///		Create a new HTTP request builder that is not attached to an <see cref="System.Net.Http.HttpClient"/>.
		/// </summary>
		/// <param name="requestUri">
		///		The request URI (can be relative or absolute).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<Unit> Create(Uri requestUri)
		{
			if (requestUri == null)
				throw new ArgumentNullException("requestUri");

			return new HttpRequestBuilder<Unit>(requestUri);
		}

		/// <summary>
		///		Create a new HTTP request builder that is not attached to an <see cref="System.Net.Http.HttpClient"/>.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object that will act as context for the request builder.
		/// </typeparam>
		/// <param name="requestUri">
		///		The request URI (can be relative or absolute).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		[SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Justification = "Calls generic type's factory method, which calls equivalent URI overload.")]
		public static HttpRequestBuilder<TContext> Create<TContext>(string requestUri)
		{
			if (String.IsNullOrWhiteSpace(requestUri))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'requestUri'.", "requestUri");

			return HttpRequestBuilder<TContext>.Create(requestUri);
		}

		/// <summary>
		///		Create a new HTTP request builder that is not attached to an <see cref="System.Net.Http.HttpClient"/>.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object that will act as context for the request builder.
		/// </typeparam>
		/// <param name="requestUri">
		///		The request URI (can be relative or absolute).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> Create<TContext>(Uri requestUri)
		{
			if (requestUri == null)
				throw new ArgumentNullException("requestUri");

			return HttpRequestBuilder<TContext>.Create(requestUri);
		}

		#region DefaultMediaTypeFormatterFactories

		/// <summary>
		///		Factory delegates used to create default <see cref="MediaTypeFormatter"/>s when none are supplied.
		/// </summary>
		public static class DefaultMediaTypeFormatterFactories
		{
			/// <summary>
			///		Factory for the default JSON media-type formatter to use when one is not supplied.
			/// </summary>
			static Func<JsonMediaTypeFormatter> _json = () => new JsonMediaTypeFormatter();

			/// <summary>
			///		Factory for the default XML media-type formatter to use when one is not supplied.
			/// </summary>
			static Func<XmlMediaTypeFormatter> _xml = () => new XmlMediaTypeFormatter();

			/// <summary>
			///		Factory for the default JSON media-type formatter to use when one is not supplied.
			/// </summary>
			public static Func<JsonMediaTypeFormatter> Json
			{
				get
				{
					return _json;
				}
				set
				{
					if (value == null)
						throw new ArgumentNullException("value", "Property cannot be null: 'Json'.");

					_json = value;
				}
			}

			/// <summary>
			///		Factory for the default XML media-type formatter to use when one is not supplied.
			/// </summary>
			public static Func<XmlMediaTypeFormatter> Xml
			{
				get
				{
					return _xml;
				}
				set
				{
					if (value == null)
						throw new ArgumentNullException("value", "Property cannot be null: 'Xml'.");

					_xml = value;
				}
			}

			/// <summary>
			///		All default media-type formatters.
			/// </summary>
			public static IEnumerable<Func<MediaTypeFormatter>> All
			{
				get
				{
					yield return _json;
					yield return _xml;
				}
			}
		}

		#endregion // DefaultMediaTypeFormatterFactories
	}
}
