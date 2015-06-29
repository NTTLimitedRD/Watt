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
		#region Invoke

		/// <summary>
		///		Asynchronously execute the request as an HTTP HEAD.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static Task<HttpResponseMessage> HeadAsync<TContext>(this IHttpRequestBuilder<TContext> requestBuilder, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Head, context))
			{
				return requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request as an HTTP GET.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> GetAsync<TContext>(this IHttpRequestBuilder<TContext> requestBuilder, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Get, context))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request as an HTTP POST.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="postBody">
		///		An optional object to be used as the the request body.
		/// </param>
		/// <param name="mediaType">
		///		If <paramref name="postBody"/> is specified, the media type to be used 
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PostAsync<TContext>(this IHttpRequestBuilder<TContext> requestBuilder, object postBody = null, string mediaType = null, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			requestBuilder.EnsureAttachedToClient();

			ObjectContent postBodyContent = null;
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

				postBodyContent = new ObjectContent(
					postBody.GetType(),
					postBody,
					mediaTypeFormatter,
					mediaType
				);
			}

			using (postBodyContent)
			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Post, context, postBodyContent))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request as an HTTP POST.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="postBody">
		///		Optional <see cref="HttpContent"/> representing the request body.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PostAsync<TContext>(this IHttpRequestBuilder<TContext> requestBuilder, HttpContent postBody = null, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Post, context, postBody))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request as an HTTP PUT.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="putBody">
		///		<see cref="HttpContent"/> representing the request body.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PutAsync<TContext>(this IHttpRequestBuilder<TContext> requestBuilder, HttpContent putBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (putBody == null)
				throw new ArgumentNullException("putBody");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Put, context, putBody))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request as an HTTP PATCH.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="patchBody">
		///		<see cref="HttpContent"/> representing the request body.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PatchAsync<TContext>(this IHttpRequestBuilder<TContext> requestBuilder, HttpContent patchBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (patchBody == null)
				throw new ArgumentNullException("patchBody");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(ExtendedHttpMethod.Patch, context, patchBody))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request as an HTTP DELETE.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> DeleteAsync<TContext>(this IHttpRequestBuilder<TContext> requestBuilder, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Delete, context))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request using the specified HTTP method.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="method">
		///		An <see cref="HttpMethod"/> representing the method to use.
		/// </param>
		/// <param name="body">
		///		Optional <see cref="HttpContent"/> representing the request body (if any).
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> SendAsync<TContext>(this IHttpRequestBuilder<TContext> requestBuilder, HttpMethod method, HttpContent body = null, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			requestBuilder.EnsureAttachedToClient();

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(method, context, body))
			{
				return await requestBuilder.HttpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously perform an HTTP POST request, serialising the request as JSON.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
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
		///		A <see cref="Task{HttpResponseMessage}"/> representing the asynchronous request, whose result is the response message.
		/// </returns>
		public static Task<HttpResponseMessage> PostAsJsonAsync<TContext>(this IHttpRequestBuilder<TContext> requestBuilder, object postBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			return requestBuilder.PostAsync(postBody, "application/json", context, cancellationToken);
		}

		#endregion // Invoke

		#region Invoke and deserialise response

		/// <summary>
		///		Asynchronously perform an HTTP GET request, deserialising the response.
		/// </summary>
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
		public static async Task<TResponse> GetAsync<TContext, TResponse>(this IHttpRequestBuilder<TContext> requestBuilder, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			HttpResponseMessage response = null;
			try
			{
				using (response = await requestBuilder.GetAsync(context, cancellationToken))
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
		public static Task<TResponse> PostAsJsonAsync<TContext, TResponse>(this IHttpRequestBuilder<TContext> requestBuilder, object postBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			return requestBuilder.PostAsync<TContext, TResponse>(postBody, "application/json", context, cancellationToken);
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
		public static Task<TResponse> PostAsXmlAsync<TContext, TResponse>(this IHttpRequestBuilder<TContext> requestBuilder, object postBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			return requestBuilder.PostAsync<TContext, TResponse>(postBody, "text/xml", context, cancellationToken);
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
		public static async Task<TResponse> PostAsync<TContext, TResponse>(this IHttpRequestBuilder<TContext> requestBuilder, object postBody, string mediaType, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpResponseMessage response = await requestBuilder.PostAsync(postBody, mediaType, context, cancellationToken))
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
		public static Task<TResponse> PutAsJsonAsync<TContext, TResponse>(this IHttpRequestBuilder<TContext> requestBuilder, object putBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			return requestBuilder.PutAsync<TContext, TResponse>(putBody, "application/json", context, cancellationToken);
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
		public static Task<TResponse> PutAsXmlAsync<TContext, TResponse>(this IHttpRequestBuilder<TContext> requestBuilder, object putBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			return requestBuilder.PutAsync<TContext, TResponse>(putBody, "text/xml", context, cancellationToken);
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
		public static async Task<TResponse> PutAsync<TContext, TResponse>(this IHttpRequestBuilder<TContext> requestBuilder, object putBody, string mediaType, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
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

				using (response = await requestBuilder.PutAsync(requestContent, context, cancellationToken))
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
