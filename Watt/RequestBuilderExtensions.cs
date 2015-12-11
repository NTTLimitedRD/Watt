using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;

namespace DD.Cloud.WebApi.TemplateToolkit
{
	/// <summary>
	///		Extension methods for <see cref="HttpRequestBuilder"/>.
	/// </summary>
	public static class RequestBuilderExtensions
	{
		/// <summary>
		///		Build and configure a new HTTP request message.
		/// </summary>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="method">
		///		The HTTP request method to use.
		/// </param>
		/// <param name="requestBody">
		///		An optional object representing representing the request body.
		/// </param>
		/// <param name="mediaType">
		///		The request body's media type.
		/// 
		///		Required if <paramref name="requestBody"/> is not <c>null</c>.
		/// </param>
		/// <param name="baseUri">
		///		An optional base URI to use if the request builder does not already have an absolute request URI.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpRequestMessage"/>.
		/// </returns>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Request content is owned by request message.")]
		public static HttpRequestMessage BuildRequestMessage(this IHttpRequestBuilder requestBuilder, HttpMethod method, object requestBody, string mediaType, Uri baseUri = null)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (method == null)
				throw new ArgumentNullException("method");

			HttpContent requestContent = null;
			try
			{
				if (requestBody != null)
				{
					if (String.IsNullOrWhiteSpace(mediaType))
						throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'contentType'.", "mediaType");

					MediaTypeFormatter mediaTypeFormatter = requestBuilder.GetMediaTypeFormatter(mediaType);
					if (mediaTypeFormatter == null)
					{
						throw new InvalidOperationException(
							String.Format(
								"None of the configured media-type formatters can handle content of type '{0}'.",
								mediaType
							)
						);
					}

					requestContent = new ObjectContent(
						requestBody.GetType(),
						requestBody,
						mediaTypeFormatter,
						mediaType
					);
				}

				return requestBuilder.BuildRequestMessage(method, requestContent, baseUri);
			}
			catch
			{
				using (requestContent)
				{
					throw;
				}
			}
		}

		/// <summary>
		///		Build and configure a new HTTP request message.
		/// </summary>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="method">
		///		The HTTP request method to use.
		/// </param>
		/// <param name="body">
		///		Optional <see cref="HttpContent"/> representing representing the request body.
		/// </param>
		/// <param name="baseUri">
		///		An optional base URI to use if the request builder does not already have an absolute request URI.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpRequestMessage"/>.
		/// </returns>
		public static HttpRequestMessage BuildRequestMessage(this IHttpRequestBuilder<Unit> requestBuilder, HttpMethod method, HttpContent body = null, Uri baseUri = null)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (method == null)
				throw new ArgumentNullException("method");

			return requestBuilder.BuildRequestMessage(method, Unit.Value, body, baseUri);
		}

		#region Configuration

		/// <summary>
		///		Create a copy of the request builder, but with the specified request-configuration action.
		/// </summary>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="requestConfiguration">
		///		A delegate that configures outgoing request messages.
		/// </param>
		/// <returns>
		///		The new HTTP request builder.
		/// </returns>
		public static HttpRequestBuilder<Unit> WithRequestConfiguration(this HttpRequestBuilder<Unit> requestBuilder, Action<HttpRequestMessage> requestConfiguration)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");
			if (requestConfiguration == null)
				throw new ArgumentNullException("requestConfiguration");
			return requestBuilder.WithRequestConfiguration(
				(request, _) => requestConfiguration(request)
			);
		}

		/// <summary>
		///		Create a copy of the request builder that adds a header to each request.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="TValue">
		///		The type of value used to populate the header.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="headerName">
		///		The header name.
		/// </param>
		/// <param name="headerValue">
		///		The header value.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithHeader<TContext, TValue>(this HttpRequestBuilder<TContext> requestBuilder, string headerName, TValue headerValue)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (String.IsNullOrWhiteSpace(headerName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", "headerName");

			if (headerValue == null)
				throw new ArgumentNullException("headerValue");

			return requestBuilder.WithHeader(
				headerName,
				ValueProvider<TContext>.FromConstantValue(headerValue)
			);
		}

		/// <summary>
		///		Create a copy of the request builder that adds a header with its value obtained from the specified delegate.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="headerName">
		///		The header name.
		/// </param>
		/// <param name="getValue">
		///		A delegate that returns the header value for each request.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithHeader<TContext>(this HttpRequestBuilder<TContext> requestBuilder, string headerName, Func<string> getValue)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (String.IsNullOrWhiteSpace(headerName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", "headerName");

			if (getValue == null)
				throw new ArgumentNullException("getValue");

			return requestBuilder.WithHeader(
				headerName,
				ValueProvider<TContext>.FromFunction(
					() => getValue()
				)
			);
		}

		/// <summary>
		///		Create a copy of the request builder that adds a header with its value obtained from the specified delegate.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="headerName">
		///		The header name.
		/// </param>
		/// <param name="valueProvider">
		///		A <see cref="IValueProvider{TSource, TValue}">value provider</see> that returns the header value for each request.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithHeader<TContext>(this HttpRequestBuilder<TContext> requestBuilder, string headerName, IValueProvider<TContext, string> valueProvider)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (String.IsNullOrWhiteSpace(headerName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", "headerName");

			if (valueProvider == null)
				throw new ArgumentNullException("valueProvider");

			return requestBuilder.WithRequestConfiguration(
				(request, context) =>
				{
					request.Headers.Remove(headerName);

					string headerValue = valueProvider.Get(context);
					if (headerValue == null)
						return;

					request.Headers.Add(headerName, headerValue);
				}
			);
		}

		/// <summary>
		///		Create a copy of the request builder that adds a header with its value obtained from the specified delegate.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="headerName">
		///		The header name.
		/// </param>
		/// <param name="getValue">
		///		A delegate that returns the header value for each request.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithHeader<TContext>(this HttpRequestBuilder<TContext> requestBuilder, string headerName, Func<TContext, string> getValue)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (String.IsNullOrWhiteSpace(headerName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", "headerName");

			if (getValue == null)
				throw new ArgumentNullException("getValue");

			return requestBuilder.WithHeader(
				headerName,
				ValueProvider<TContext>.FromSelector(
					context => getValue(context)
				)
			);
		}

		/// <summary>
		///		Create a copy of the request builder that adds an "If-None-Match" header to each request.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="headerValue">
		///		The header value.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithIfNoneMatchHeader<TContext>(this HttpRequestBuilder<TContext> requestBuilder, string headerValue)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException(nameof(requestBuilder));

			if (headerValue == null)
				throw new ArgumentNullException(nameof(headerValue));

			return requestBuilder.WithHeader("If-None-Match", _ => headerValue);
		}

		/// <summary>
		///		Create a copy of the request builder that adds an "If-None-Match" header with its value obtained from the specified delegate.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="getValue">
		///		A delegate that returns the header value for each request.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithIfNoneMatchHeader<TContext>(this HttpRequestBuilder<TContext> requestBuilder, Func<TContext, string> getValue)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException(nameof(requestBuilder));

			if (getValue == null)
				throw new ArgumentNullException(nameof(getValue));

			return requestBuilder.WithHeader("If-None-Match", getValue);
		}

		/// <summary>
		///		Create a copy of the request builder with the specified request URI template parameter.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="T">
		///		The parameter data-type.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="name">
		///		The parameter name.
		/// </param>
		/// <param name="value">
		///		The parameter value.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithTemplateParameter<TContext, T>(this HttpRequestBuilder<TContext> requestBuilder, string name, T value)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", "name");

			return requestBuilder.WithTemplateParameter(
				name,
				getValue: context => value
			);
		}

		/// <summary>
		///		Create a copy of the request builder, but with the specified template parameters.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="TParameters">
		///		The type whose public properties are used as template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="parameters">
		///		An object whose properties are used as template parameters.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithTemplateParameters<TContext, TParameters>(this HttpRequestBuilder<TContext> requestBuilder, TParameters parameters)
			where TParameters : class
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (parameters == null)
				throw new ArgumentNullException("parameters");

			IDictionary<string, IValueProvider<TContext, string>> templateParameters = parameters.ToDeferredParameterDictionary<TContext, TParameters>();
			
			return requestBuilder.WithTemplateParameters(templateParameters);
		}

		/// <summary>
		///		Create a copy of the request builder that is configured to expect JSON responses.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="onlyJson">
		///		Only expect JSON?
		/// 
		///		If <c>true</c>, all other Accept header values will be removed.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> ExpectJson<TContext>(this HttpRequestBuilder<TContext> requestBuilder, bool onlyJson = false)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			return requestBuilder.WithRequestConfiguration(
				(request, context) =>
				{
					if (request.Headers.Accept.Any(accept => accept.MediaType == "application/json"))
						return;

					if (onlyJson)
						request.Headers.Accept.Clear();

					request.Headers.Accept.Add(
						new MediaTypeWithQualityHeaderValue("application/json")
					);
				}
			);
		}

		/// <summary>
		///		Create a copy of the request builder that is configured to expect XML responses.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="onlyXml">
		///		Only expect XML?
		/// 
		///		If <c>true</c>, all other Accept header values will be removed.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> ExpectXml<TContext>(this HttpRequestBuilder<TContext> requestBuilder, bool onlyXml = false)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			return requestBuilder.WithRequestConfiguration(
				(request, context) =>
				{
					if (request.Headers.Accept.Any(accept => accept.MediaType == "text/xml"))
						return;

					if (onlyXml)
						request.Headers.Accept.Clear();

					request.Headers.Accept.Add(
						new MediaTypeWithQualityHeaderValue("text/xml")
					);
				}
			);
		}

		/// <summary>
		///		Create a copy of the request builder that is configured to use JSON for requests and responses.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="jsonFormatter">
		///		An optional JSON media-type formatter to use.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> UseJson<TContext>(this HttpRequestBuilder<TContext> requestBuilder, JsonMediaTypeFormatter jsonFormatter = null)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (jsonFormatter == null)
				jsonFormatter = HttpRequestBuilder.DefaultMediaTypeFormatterFactories.Json();

			if (jsonFormatter == null)
				throw new InvalidOperationException("DefaultMediaTypeFormatterFactories.Json returned null.");

			return
				requestBuilder
					.WithMediaTypeFormatters(jsonFormatter)
					.ExpectJson(onlyJson: true);
		}

		/// <summary>
		///		Create a copy of the request builder that is configured to use XML for requests and responses.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="xmlFormatter">
		///		An optional XML media-type formatter to use.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> UseXml<TContext>(this HttpRequestBuilder<TContext> requestBuilder, XmlMediaTypeFormatter xmlFormatter = null)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (xmlFormatter == null)
				xmlFormatter = HttpRequestBuilder.DefaultMediaTypeFormatterFactories.Xml();

			if (xmlFormatter == null)
				throw new InvalidOperationException("DefaultMediaTypeFormatterFactories.Xml returned null.");

			return
				requestBuilder
					.WithMediaTypeFormatters(xmlFormatter)
					.ExpectXml(onlyXml: true);
		}

		/// <summary>
		///		Create a copy of the request builder with the specified query parameter.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="T">
		///		The parameter value type.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="name">
		///		The query parameter name.
		/// </param>
		/// <param name="value">
		///		The query parameter value.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithQueryParameter<TContext, T>(this HttpRequestBuilder<TContext> requestBuilder, string name, T value)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", "name");

			return requestBuilder.WithQueryParameter(
				name,
				ValueProvider<TContext>.FromConstantValue(value)
			);
		}

		/// <summary>
		///		Create a copy of the request builder, but with the specified query parameters.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="TParameters">
		///		The type whose public properties are used as query parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="parameters">
		///		An object whose properties are used as query parameters.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithQueryParameters<TContext, TParameters>(this HttpRequestBuilder<TContext> requestBuilder, TParameters parameters)
			where TParameters : class
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (parameters == null)
				throw new ArgumentNullException("parameters");

			IDictionary<string, IValueProvider<TContext, string>> queryParameters = parameters.ToDeferredParameterDictionary<TContext, TParameters>();

			return requestBuilder.WithQueryParameters(queryParameters);
		}

		/// <summary>
		///		Create a copy of the request builder, but with the default media-type formatters.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithDefaultMediaTypeFormatters<TContext>(this HttpRequestBuilder<TContext> requestBuilder)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			return requestBuilder.WithMediaTypeFormatters(
				HttpRequestBuilder.DefaultMediaTypeFormatterFactories.All.Select(
					mediaTypeFormatterFactory => mediaTypeFormatterFactory()
				)
			);
		}

		/// <summary>
		///		Create a copy of the request builder, but with no media-type formatters.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithNoMediaTypeFormatters<TContext>(this HttpRequestBuilder<TContext> requestBuilder)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			return requestBuilder.WithMediaTypeFormatters(
				Enumerable.Empty<MediaTypeFormatter>()
			);
		}

		/// <summary>
		///		Create a copy of the request builder, but with the specified media-type formatters.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="mediaTypeFormatters">
		///		The media-type formatters to use.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithMediaTypeFormatters<TContext>(this HttpRequestBuilder<TContext> requestBuilder, params MediaTypeFormatter[] mediaTypeFormatters)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (mediaTypeFormatters == null)
				throw new ArgumentNullException("mediaTypeFormatters");

			return requestBuilder.WithMediaTypeFormatters(mediaTypeFormatters);
		}

		/// <summary>
		///		Create a copy of the request builder, but with the specified media-type formatters appended to its existing list of media-type formatters.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="mediaTypeFormatters">
		///		The media-type formatters to use.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithAdditionalMediaTypeFormatters<TContext>(this HttpRequestBuilder<TContext> requestBuilder, params MediaTypeFormatter[] mediaTypeFormatters)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (mediaTypeFormatters == null)
				throw new ArgumentNullException("mediaTypeFormatters");

			return requestBuilder.WithAdditionalMediaTypeFormatters(mediaTypeFormatters);
		}

		/// <summary>
		///		Create a copy of the request builder, but without the specified media-type formatters.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="mediaTypeFormatters">
		///		The media-type formatters to remove.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithoutMediaTypeFormatters<TContext>(this HttpRequestBuilder<TContext> requestBuilder, params MediaTypeFormatter[] mediaTypeFormatters)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (mediaTypeFormatters == null)
				throw new ArgumentNullException("mediaTypeFormatters");

			return requestBuilder.WithoutMediaTypeFormatters(mediaTypeFormatters);
		}

		#endregion // Configuration

		#region Media type formatters

		/// <summary>
		///		Get the first media-type formatter that can handle the specified media type.
		/// </summary>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="mediaType">
		///		The media type to match.
		/// </param>
		/// <returns>
		///		The media-type formatter, or <c>null</c> if none of the request builder's configured media-type formatters can handle the specified media type.
		/// </returns>
		public static MediaTypeFormatter GetMediaTypeFormatter(this IHttpRequestBuilder requestBuilder, string mediaType)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (String.IsNullOrWhiteSpace(mediaType))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'mediaType'.", "mediaType");

			return requestBuilder.MediaTypeFormatters.FirstOrDefault(
				formatter => formatter.MediaTypeMappings.Any(
					mapping => mapping.MediaType.MediaType == mediaType
				)
			);
		}

		#endregion // Media type formatters

		#region Helpers

		/// <summary>
		///		Convert the specified object to a deferred parameter dictionary.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="TParameters">
		///		The type of object whose properties will form the parameters.
		/// </typeparam>
		/// <param name="parameters">
		///		The object whose properties will form the parameters.
		/// </param>
		/// <returns>
		///		The dictionary.
		/// </returns>
		/// <remarks>
		///		The returned dictionary is case-insensitive.
		/// </remarks>
		static IDictionary<string, IValueProvider<TContext, string>> ToDeferredParameterDictionary<TContext, TParameters>(this TParameters parameters)
		{
			if (Equals(parameters, null))
				throw new ArgumentNullException("parameters");

			// Reflection might be "slow", but it's still blazingly fast compared to making a request over the network.
			Dictionary<string, IValueProvider<TContext, string>> parameterDictionary = new Dictionary<string, IValueProvider<TContext, string>>(StringComparer.OrdinalIgnoreCase);
			foreach (PropertyInfo property in typeof(TParameters).GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				// Ignore write-only properties.
				if (!property.CanRead)
					continue;

				parameterDictionary[property.Name] =
					ValueProvider<TContext>.FromSelector(
						context => property.GetValue(parameters)
					)
					.Convert().ValueToString();
			}

			return parameterDictionary;
		}

		#endregion // Helpers
	}
}
