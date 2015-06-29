using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Formatting;

namespace DD.Cloud.WebApi.TemplateToolkit
{
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

			return new HttpRequestBuilder<Unit>(
				httpClient: null,
				requestUri: requestUri
			);
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
	}
}
