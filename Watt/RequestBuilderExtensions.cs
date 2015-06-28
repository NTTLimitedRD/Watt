using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DD.Cloud.WebApi.TemplateToolkit
{
	/// <summary>
	///		Extension methods for <see cref="HttpRequestBuilder"/>.
	/// </summary>
	public static class RequestBuilderExtensions
	{
		#region Invocation

		/// <summary>
		///		Asynchronously execute the request as an HTTP HEAD.
		/// </summary>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static Task<HttpResponseMessage> HeadAsync(this IHttpRequestBuilder requestBuilder, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Head))
			{
				return requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request as an HTTP GET.
		/// </summary>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> GetAsync(this IHttpRequestBuilder requestBuilder, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Get))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request as an HTTP POST.
		/// </summary>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="postBody">
		///		Optional <see cref="HttpContent"/> representing the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PostAsync(this IHttpRequestBuilder requestBuilder, HttpContent postBody = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Post, postBody))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request as an HTTP PUT.
		/// </summary>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="putBody">
		///		<see cref="HttpContent"/> representing the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PutAsync(this IHttpRequestBuilder requestBuilder, HttpContent putBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (putBody == null)
				throw new ArgumentNullException("putBody");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Put, putBody))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request as an HTTP PATCH.
		/// </summary>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="patchBody">
		///		<see cref="HttpContent"/> representing the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PatchAsync(this IHttpRequestBuilder requestBuilder, HttpContent patchBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (patchBody == null)
				throw new ArgumentNullException("patchBody");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(ExtendedHttpMethod.Patch, patchBody))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request as an HTTP DELETE.
		/// </summary>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> DeleteAsync(this IHttpRequestBuilder requestBuilder, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Delete))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request using the specified HTTP method.
		/// </summary>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="method">
		///		An <see cref="HttpMethod"/> representing the method to use.
		/// </param>
		/// <param name="body">
		///		Optional <see cref="HttpContent"/> representing the request body (if any).
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> SendAsync(this IHttpRequestBuilder requestBuilder, HttpMethod method, HttpContent body = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(method, body))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

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

		/// <summary>
		///		Asynchronously perform an HTTP GET request, deserialising the response.
		/// </summary>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static async Task<TResponse> GetAsync<TResponse>(this IHttpRequestBuilder requestBuilder, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			HttpResponseMessage response = null;
			try
			{
				using (response = await requestBuilder.GetAsync(cancellationToken))
				{
					if (response.StatusCode == HttpStatusCode.NoContent || response.Content == null)
						return default(TResponse);

					response.EnsureSuccessStatusCode();

					return await response.Content.ReadAsAsync<TResponse>(requestBuilder.MediaTypeFormatters, cancellationToken);
				}
			}
			catch
			{
				using (response)
				{
					throw;
				}
			}
		}

		/// <summary>
		///		Asynchronously perform an HTTP POST request, serialising the request as JSON, and deserialising the response.
		/// </summary>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="postBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static Task<TResponse> PostAsJsonAsync<TResponse>(this IHttpRequestBuilder requestBuilder, object postBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return requestBuilder.PostAsync<TResponse>(postBody, "application/json", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP POST request, serialising the request as XML, and deserialising the response (which is expected to be XML).
		/// </summary>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="postBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static Task<TResponse> PostAsXmlAsync<TResponse>(this IHttpRequestBuilder requestBuilder, object postBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return requestBuilder.PostAsync<TResponse>(postBody, "text/xml", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP POST request, serialising the request, and deserialising the response.
		/// </summary>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="postBody">
		///		The object that will be serialised into the response body.
		/// </param>
		/// <param name="mediaType">
		///		The request content type.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static async Task<TResponse> PostAsync<TResponse>(this IHttpRequestBuilder requestBuilder, object postBody, string mediaType, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			HttpResponseMessage response = null;
			ObjectContent requestContent = null;
			try
			{
				if (postBody != null)
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
						postBody.GetType(),
						postBody,
						mediaTypeFormatter,
						mediaType
					);
				}

				using (response = await requestBuilder.PostAsync(requestContent, cancellationToken))
				{
					if (response.StatusCode == HttpStatusCode.NoContent || response.Content == null)
						return default(TResponse);

					response.EnsureSuccessStatusCode();

					return await response.Content.ReadAsAsync<TResponse>(requestBuilder.MediaTypeFormatters, cancellationToken);
				}
			}
			catch
			{
				using (requestContent)
				using (response)
				{
					throw;
				}
			}
		}

		/// <summary>
		///		Asynchronously perform an HTTP PUT request, serialising the request as JSON, and deserialising the response.
		/// </summary>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="putBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static Task<TResponse> PutAsJsonAsync<TResponse>(this IHttpRequestBuilder requestBuilder, object putBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return requestBuilder.PutAsync<TResponse>(putBody, "application/json", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP PUT request, serialising the request as XML, and deserialising the response.
		/// </summary>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="putBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static Task<TResponse> PutAsXmlAsync<TResponse>(this IHttpRequestBuilder requestBuilder, object putBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return requestBuilder.PutAsync<TResponse>(putBody, "text/xml", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP PUT request, serialising the request as JSON, and deserialising the response (which is expected to be JSON).
		/// </summary>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="putBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="mediaType">
		///		The request content type.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static async Task<TResponse> PutAsync<TResponse>(this IHttpRequestBuilder requestBuilder, object putBody, string mediaType, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			HttpResponseMessage response = null;
			ObjectContent requestContent = null;
			try
			{
				if (putBody != null)
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
						putBody.GetType(),
						putBody,
						mediaTypeFormatter,
						mediaType
					);
				}

				using (response = await requestBuilder.PutAsync(requestContent, cancellationToken))
				{
					if (response.StatusCode == HttpStatusCode.NoContent || response.Content == null)
						return default(TResponse);

					response.EnsureSuccessStatusCode();

					return await response.Content.ReadAsAsync<TResponse>(requestBuilder.MediaTypeFormatters, cancellationToken);
				}
			}
			catch
			{
				using (requestContent)
				using (response)
				{
					throw;
				}
			}
		}

		#endregion // Invocation

		#region Configuration

		/// <summary>
		///		Create a copy of the request builder that adds a header to each request.
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
		/// <param name="headerValue">
		///		The header value.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequestBuilder{TContext}"/>.
		/// </returns>
		public static HttpRequestBuilder<TContext> WithHeader<TContext>(this HttpRequestBuilder<TContext> requestBuilder, string headerName, string headerValue)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (String.IsNullOrWhiteSpace(headerName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", "headerName");

			if (headerValue == null)
				throw new ArgumentNullException("headerValue");

			return requestBuilder.WithHeader(
				headerName,
				context => headerValue
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
				context => getValue()
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

			return requestBuilder.WithRequestConfiguration(
				(request, context) =>
				{
					request.Headers.Remove(headerName);

					string headerValue = getValue(context);
					if (headerValue == null)
						return;

					request.Headers.Add(headerName, headerValue);
				}
			);
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

			IDictionary<string, Func<TContext, string>> templateParameters = parameters.ToDeferredParameterDictionary<TContext, TParameters>();
			
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

			return
				requestBuilder.WithMediaTypeFormatters(
					jsonFormatter ?? new JsonMediaTypeFormatter()
				)
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

			return
				requestBuilder.WithMediaTypeFormatters(
					xmlFormatter ?? new XmlMediaTypeFormatter()
				)
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

			string parameterValue = value != null ? value.ToString() : null;

			return requestBuilder.WithQueryParameter(
				name,
				getValue: () => parameterValue
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

			IDictionary<string, Func<TContext, string>> queryParameters = parameters.ToDeferredParameterDictionary<TContext, TParameters>();

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
				new JsonMediaTypeFormatter(),
				new XmlMediaTypeFormatter()
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

		/// <summary>
		///		Create a copy of the request builder, but with the specified default context.
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
		public static HttpRequestBuilder<TContext> WithoutContext<TContext>(this HttpRequestBuilder<TContext> requestBuilder)
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			TContext noContext = default(TContext);
			
			return
				!Equals(requestBuilder.Context, noContext) ?
					requestBuilder.WithContext(noContext)
					:
					requestBuilder;
		}

		#endregion // Configuration

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
		///		The returned dictionary is case-insensitive.
		/// </returns>
		static IDictionary<string, Func<TContext, string>> ToDeferredParameterDictionary<TContext, TParameters>(this TParameters parameters)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			Dictionary<string, Func<TContext, string>> parameterDictionary = new Dictionary<string, Func<TContext, string>>(StringComparer.OrdinalIgnoreCase);
			foreach (PropertyInfo property in typeof(TParameters).GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				// Ignore write-only properties.
				if (!property.CanRead)
					continue;

				parameterDictionary[property.Name] = context =>
				{
					object value = property.GetValue(parameters);

					return value != null ? value.ToString() : null;
				};
			}

			return parameterDictionary;
		}

		#endregion // Helpers
	}
}
