using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace DD.Cloud.WebApi.TemplateToolkit
{
	/// <summary>
	///		Extension methods for <see cref="HttpClient"/> and <see cref="HttpRequestBuilder"/>.
	/// </summary>
	public static class HttpClientExtensions
	{
		#region Invoke

		/// <summary>
		///		Asynchronously execute a request as an HTTP HEAD.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> HeadAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Head, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP GET.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Get, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP POST.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="postBody">
		///		An optional object to be used as the the request body.
		/// </param>
		/// <param name="mediaType">
		///		If <paramref name="postBody"/> is specified, the media type to be used 
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object postBody = null, string mediaType = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Post, requestBody: postBody, mediaType: mediaType, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP POST.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, HttpContent postBody = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Post, postBody, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP PUT.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="putBody">
		///		An optional object to be used as the the request body.
		/// </param>
		/// <param name="mediaType">
		///		If <paramref name="putBody"/> is specified, the media type to be used 
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object putBody = null, string mediaType = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Put, putBody, mediaType, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP PUT.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		public static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, HttpContent putBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (putBody == null)
				throw new ArgumentNullException("putBody");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Put, putBody, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP PATCH.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="patchBody">
		///		An optional object to be used as the the request body.
		/// </param>
		/// <param name="mediaType">
		///		If <paramref name="patchBody"/> is specified, the media type to be used 
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object patchBody = null, string mediaType = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(OtherHttpMethods.Patch, patchBody, mediaType, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP PATCH.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		public static async Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, HttpContent patchBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (patchBody == null)
				throw new ArgumentNullException("patchBody");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(OtherHttpMethods.Patch, patchBody, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP DELETE.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Delete, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request using the specified HTTP method.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, HttpMethod method, HttpContent body = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(method, body, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously perform an HTTP POST request, serialising the request to JSON.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		///		A <see cref="Task{HttpResponseMessage}"/> representing the asynchronous request, whose result is the response message.
		/// </returns>
		public static Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object postBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PostAsync(requestBuilder, postBody, "application/json", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP PUT request, serialising the request to JSON.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		///		A <see cref="Task{HttpResponseMessage}"/> representing the asynchronous request, whose result is the response message.
		/// </returns>
		public static Task<HttpResponseMessage> PutAsJsonAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object putBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PutAsync(requestBuilder, putBody, "application/json", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP PATCH request, serialising the request to JSON.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="patchBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{HttpResponseMessage}"/> representing the asynchronous request, whose result is the response message.
		/// </returns>
		public static Task<HttpResponseMessage> PatchAsJsonAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object patchBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PatchAsync(requestBuilder, patchBody, "application/json", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP POST request, serialising the request to XML.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		///		A <see cref="Task{HttpResponseMessage}"/> representing the asynchronous request, whose result is the response message.
		/// </returns>
		public static Task<HttpResponseMessage> PostAsXmlAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object postBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PostAsync(requestBuilder, postBody, "text/xml", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP PUT request, serialising the request to XML.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		///		A <see cref="Task{HttpResponseMessage}"/> representing the asynchronous request, whose result is the response message.
		/// </returns>
		public static Task<HttpResponseMessage> PutAsXmlAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object putBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PutAsync(requestBuilder, putBody, "text/xml", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP PATCH request, serialising the request to XML.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="patchBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{HttpResponseMessage}"/> representing the asynchronous request, whose result is the response message.
		/// </returns>
		public static Task<HttpResponseMessage> PatchAsXmlAsync(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object patchBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PatchAsync(requestBuilder, patchBody, "text/xml", cancellationToken);
		}

		#endregion // Invoke

		#region Invoke with context

		/// <summary>
		///		Asynchronously execute a request as an HTTP HEAD.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		public static async Task<HttpResponseMessage> HeadAsync<TContext>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Head, context, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP GET.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		public static async Task<HttpResponseMessage> GetAsync<TContext>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Get, context, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP POST.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		public static async Task<HttpResponseMessage> PostAsync<TContext>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, object postBody = null, string mediaType = null, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

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
			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Post, context, postBodyContent, httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP POST.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		public static async Task<HttpResponseMessage> PostAsync<TContext>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, HttpContent postBody = null, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Post, context, postBody, httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP PUT.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		public static async Task<HttpResponseMessage> PutAsync<TContext>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, HttpContent putBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (putBody == null)
				throw new ArgumentNullException("putBody");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Put, context, putBody, httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP PATCH.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		public static async Task<HttpResponseMessage> PatchAsync<TContext>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, HttpContent patchBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			if (patchBody == null)
				throw new ArgumentNullException("patchBody");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(OtherHttpMethods.Patch, context, patchBody, httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP DELETE.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		public static async Task<HttpResponseMessage> DeleteAsync<TContext>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(HttpMethod.Delete, context, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request using the specified HTTP method.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
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
		public static async Task<HttpResponseMessage> SendAsync<TContext>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, HttpMethod method, HttpContent body = null, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpRequestMessage request = requestBuilder.BuildRequestMessage(method, context, body, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(request, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously perform an HTTP POST request, serialising the request as JSON.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
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
		///		A <see cref="Task{HttpResponseMessage}"/> representing the asynchronous request, whose result is the response message.
		/// </returns>
		public static Task<HttpResponseMessage> PostAsJsonAsync<TContext>(this HttpClient httpClient, IHttpRequestBuilder<TContext> requestBuilder, object postBody, TContext context = default(TContext), CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			return httpClient.PostAsync(requestBuilder, postBody, "application/json", context, cancellationToken);
		}

		#endregion // Invoke with context

		#region Invoke and deserialise response

		/// <summary>
		///		Asynchronously perform an HTTP GET request, deserialising the response.
		/// </summary>
		/// <typeparam name="TResponse">
		///		The type into to which the response should be deserialised.
		/// </typeparam>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="requestBuilder">
		///		The HTTP request builder.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static async Task<TResponse> GetAsync<TResponse>(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			HttpResponseMessage response = null;
			try
			{
				using (response = await httpClient.GetAsync(requestBuilder, cancellationToken))
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
		///		Asynchronously perform an HTTP POST request, serialising the request to JSON, and deserialising the response.
		/// </summary>
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
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static Task<TResponse> PostAsJsonAsync<TResponse>(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object postBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PostAsync<TResponse>(requestBuilder, postBody, "application/json", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP POST request, serialising the request to XML, and deserialising the response (which is expected to be XML).
		/// </summary>
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
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static Task<TResponse> PostAsXmlAsync<TResponse>(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object postBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PostAsync<TResponse>(requestBuilder, postBody, "text/xml", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP POST request, serialising the request, and deserialising the response.
		/// </summary>
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
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static async Task<TResponse> PostAsync<TResponse>(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object postBody, string mediaType, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (requestBuilder == null)
				throw new ArgumentNullException("requestBuilder");

			using (HttpResponseMessage response = await httpClient.PostAsync(requestBuilder, postBody, mediaType, cancellationToken))
			{
				if (response.StatusCode == HttpStatusCode.NoContent || response.Content == null)
					return default(TResponse);

				response.EnsureSuccessStatusCode();

				return await response.Content.ReadAsAsync<TResponse>(requestBuilder.MediaTypeFormatters, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously perform an HTTP PUT request, serialising the request to JSON, and deserialising the response.
		/// </summary>
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
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static Task<TResponse> PutAsJsonAsync<TResponse>(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object putBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PutAsync<TResponse>(requestBuilder, putBody, "application/json", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP PUT request, serialising the request to XML, and deserialising the response.
		/// </summary>
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
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static Task<TResponse> PutAsXmlAsync<TResponse>(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object putBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PutAsync<TResponse>(requestBuilder, putBody, "text/xml", cancellationToken);
		}

		/// <summary>
		///		Asynchronously perform an HTTP PUT request, serialising the request to JSON, and deserialising the response (which is expected to be JSON).
		/// </summary>
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
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous request, whose result is the deserialised response.
		/// </returns>
		public static async Task<TResponse> PutAsync<TResponse>(this HttpClient httpClient, IHttpRequestBuilder requestBuilder, object putBody, string mediaType, CancellationToken cancellationToken = default(CancellationToken))
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

				using (response = await httpClient.PutAsync(requestBuilder, requestContent, cancellationToken))
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
