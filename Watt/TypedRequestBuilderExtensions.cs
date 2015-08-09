using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace DD.Cloud.WebApi.TemplateToolkit
{
	/// <summary>
	///		Extension methods for <see cref="IHttpRequestBuilder{TContext}"/>.
	/// </summary>
	public static class TypedRequestBuilderExtensions
	{
		/// <summary>
		///		Build and configure a new HTTP request message.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="method">
		///		The HTTP request method to use.
		/// </param>
		/// <param name="context">
		///		An optional <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
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
		public static HttpRequestMessage BuildRequestMessage<TContext>(this IHttpRequestBuilder<TContext> requestBuilder, HttpMethod method, TContext context, object requestBody, string mediaType, Uri baseUri = null)
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

				return requestBuilder.BuildRequestMessage(method, context, requestContent, baseUri);
			}
			catch
			{
				using (requestContent)
				{
					throw;
				}
			}
		}

		#region Invoke and deserialise response

		/// <summary>
		///		Asynchronously perform an HTTP GET request, deserialising the response.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static async Task<TResponse> GetAsync<TContext, TResponse>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			HttpResponseMessage response = null;
			try
			{
				using (response = await httpClient.GetAsync(requestBuilder, context, cancellationToken))
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
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="postBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static Task<TResponse> PostAsJsonAsync<TContext, TResponse>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, object postBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			return httpClient.PostAsync<TContext, TResponse>(requestBuilder, postBody, "application/json", context, cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP POST request, serialising the request as XML, and deserialising the response (which is expected to be XML).
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="postBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static Task<TResponse> PostAsXmlAsync<TContext, TResponse>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, object postBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			return httpClient.PostAsync<TContext, TResponse>(requestBuilder, postBody, "text/xml", context, cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP POST request, serialising the request, and deserialising the response.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="postBody">
		///		The object that will be serialised into the response body.
		/// </param>
		/// <param name="mediaType">
		///		The request content type.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static async Task<TResponse> PostAsync<TContext, TResponse>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, object postBody, string mediaType, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpResponseMessage response = await httpClient.PostAsync(requestBuilder, postBody, mediaType, context, cancellationToken))
			{
				if (response.StatusCode == HttpStatusCode.NoContent || response.Content == null)
					return default(TResponse);

				response.EnsureSuccessStatusCode();

				return await response.Content.ReadAsAsync<TResponse>(requestBuilder.MediaTypeFormatters, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously perform an HTTP PUT request, serialising the request as JSON, and deserialising the response.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="putBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static Task<TResponse> PutAsJsonAsync<TContext, TResponse>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, object putBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			return httpClient.PutAsync<TContext, TResponse>(requestBuilder, putBody, "application/json", context, cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP PUT request, serialising the request as XML, and deserialising the response.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="putBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static Task<TResponse> PutAsXmlAsync<TContext, TResponse>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, object putBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			return httpClient.PutAsync<TContext, TResponse>(requestBuilder, putBody, "text/xml", context, cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP PUT request, serialising the request as JSON, and deserialising the response (which is expected to be JSON).
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="putBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="mediaType">
		///		The request content type.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static async Task<TResponse> PutAsync<TContext, TResponse>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, object putBody, string mediaType, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

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

				using (response = await httpClient.PutAsync(requestBuilder, requestContent, context, cancellationToken))
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

		#endregion // Invoke and deserialise response
	}
}
