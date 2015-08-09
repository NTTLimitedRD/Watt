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
	}
}
