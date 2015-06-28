using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DD.Cloud.WebApi.TemplateToolkit
{
	/// <summary>
	///		Extension methods for <see cref="HttpClient"/> and <see cref="HttpRequestBuilder"/>.
	/// </summary>
	public static class HttpClientExtensions
	{
		/// <summary>
		///		Build a request for the <see cref="HttpClient"/>.
		/// </summary>
		/// <param name="httpClient">
		///		The HTTP client.
		/// </param>
		/// <param name="requestUri">
		///		The request URI.
		/// </param>
		/// <returns>
		///		An <see cref="HttpRequestBuilder"/> that can be used to invoke the request or perform further configuration.
		/// </returns>
		public static HttpRequestBuilder<Unit> BuildRequest(this HttpClient httpClient, Uri requestUri)
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestUri == null)
				throw new ArgumentNullException("requestUri");

			return httpClient.BuildRequest<Unit>(requestUri);
		}

		/// <summary>
		///		Build a request for the <see cref="HttpClient"/>.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="httpClient">
		///		The HTTP client.
		/// </param>
		/// <param name="requestUri">
		///		The request URI.
		/// </param>
		/// <returns>
		///		An <see cref="HttpRequestBuilder{TContext}"/> that can be used to invoke the request or perform further configuration.
		/// </returns>
		public static HttpRequestBuilder<TContext> BuildRequest<TContext>(this HttpClient httpClient, Uri requestUri)
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestUri == null)
				throw new ArgumentNullException("requestUri");

			return new HttpRequestBuilder<TContext>(httpClient, requestUri).WithDefaultMediaTypeFormatters();
		}

		/// <summary>
		///		Build a request for the <see cref="HttpClient"/> that uses JSON.
		/// </summary>
		/// <param name="httpClient">
		///		The HTTP client.
		/// </param>
		/// <param name="requestUri">
		///		The request URI.
		/// </param>
		/// <param name="mediaTypeFormatter">
		///		An optional <see cref="JsonMediaTypeFormatter"/> to use (otherwise a new <see cref="JsonMediaTypeFormatter"/> is used).
		/// </param>
		/// <returns>
		///		An <see cref="HttpRequestBuilder"/> that can be used to invoke the request or perform further configuration.
		/// </returns>
		public static HttpRequestBuilder<Unit> BuildJsonRequest(this HttpClient httpClient, Uri requestUri, JsonMediaTypeFormatter mediaTypeFormatter = null)
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestUri == null)
				throw new ArgumentNullException("requestUri");

			return httpClient.BuildJsonRequest<Unit>(requestUri, mediaTypeFormatter);
		}

		/// <summary>
		///		Build a request for the <see cref="HttpClient"/> that uses JSON.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="httpClient">
		///		The HTTP client.
		/// </param>
		/// <param name="requestUri">
		///		The request URI.
		/// </param>
		/// <param name="mediaTypeFormatter">
		///		An optional <see cref="JsonMediaTypeFormatter"/> to use (otherwise a new <see cref="JsonMediaTypeFormatter"/> is used).
		/// </param>
		/// <returns>
		///		An <see cref="HttpRequestBuilder{TContext}"/> that can be used to invoke the request or perform further configuration.
		/// </returns>
		public static HttpRequestBuilder<TContext> BuildJsonRequest<TContext>(this HttpClient httpClient, Uri requestUri, JsonMediaTypeFormatter mediaTypeFormatter = null)
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestUri == null)
				throw new ArgumentNullException("requestUri");

			return new HttpRequestBuilder<TContext>(httpClient, requestUri).UseJson(mediaTypeFormatter);
		}

		/// <summary>
		///		Build a request for the <see cref="HttpClient"/> that uses XML.
		/// </summary>
		/// <param name="httpClient">
		///		The HTTP client.
		/// </param>
		/// <param name="requestUri">
		///		The request URI.
		/// </param>
		/// <param name="useXmlSerializer">
		///		Use the <see cref="XmlSerializer"/> instead of the <see cref="DataContractSerializer"/>?
		/// </param>
		/// <returns>
		///		An <see cref="HttpRequestBuilder"/> that can be used to invoke the request or perform further configuration.
		/// </returns>
		public static HttpRequestBuilder<Unit> BuildXmlRequest(this HttpClient httpClient, Uri requestUri, bool useXmlSerializer = false)
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestUri == null)
				throw new ArgumentNullException("requestUri");

			return httpClient.BuildXmlRequest<Unit>(requestUri, useXmlSerializer);
		}

		/// <summary>
		///		Build a request for the <see cref="HttpClient"/> that uses XML.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used by the request builder when resolving deferred template parameters.
		/// </typeparam>
		/// <param name="httpClient">
		///		The HTTP client.
		/// </param>
		/// <param name="requestUri">
		///		The request URI.
		/// </param>
		/// <param name="useXmlSerializer">
		///		Use the <see cref="XmlSerializer"/> instead of the <see cref="DataContractSerializer"/>?
		/// </param>
		/// <returns>
		///		An <see cref="HttpRequestBuilder{TContext}"/> that can be used to invoke the request or perform further configuration.
		/// </returns>
		public static HttpRequestBuilder<TContext> BuildXmlRequest<TContext>(this HttpClient httpClient, Uri requestUri, bool useXmlSerializer = false)
		{
			if (httpClient == null)
				throw new ArgumentNullException("httpClient");

			if (requestUri == null)
				throw new ArgumentNullException("requestUri");

			return new HttpRequestBuilder<TContext>(httpClient, requestUri).UseXml(
				new XmlMediaTypeFormatter
				{
					UseXmlSerializer = useXmlSerializer
				}
			);
		}
	}
}
