using System;
using System.Collections.Specialized;
using System.Text;

namespace DD.Cloud.WebApi.TemplateToolkit.Utilities
{
	/// <summary>
	///		Helper methods for working with <see cref="Uri"/>s.
	/// </summary>
	public static class UriHelper
	{
		/// <summary>
		///		Create a copy of URI with its <see cref="Uri.Query">query</see> component populated with the supplied parameters.
		/// </summary>
		/// <param name="uri">
		///		The <see cref="Uri"/> used to construct the URI.
		/// </param>
		/// <param name="parameters">
		///		A <see cref="NameValueCollection"/> representing the query parameters.
		/// </param>
		/// <returns>
		///		A new URI with the specified query.
		/// </returns>
		public static Uri WithQueryParameters(this Uri uri, NameValueCollection parameters)
		{
			if (uri == null)
				throw new ArgumentNullException("uri");

			if (parameters == null)
				throw new ArgumentNullException("parameters");

			return
				new UriBuilder(uri)
					.WithQueryParameters(parameters)
					.Uri;
		}

		/// <summary>
		///		Populate the <see cref="UriBuilder.Query">query</see> component of the URI.
		/// </summary>
		/// <param name="uriBuilder">
		///		The <see cref="UriBuilder"/> used to construct the URI
		/// </param>
		/// <param name="parameters">
		///		A <see cref="NameValueCollection"/> representing the query parameters.
		/// </param>
		/// <returns>
		///		The <paramref name="uriBuilder">URI builder</paramref> (enables inline use).
		/// </returns>
		public static UriBuilder WithQueryParameters(this UriBuilder uriBuilder, NameValueCollection parameters)
		{
			if (uriBuilder == null)
				throw new ArgumentNullException("uriBuilder");

			if (parameters == null)
				throw new ArgumentNullException("parameters");

			if (parameters.Count == 0)
				return uriBuilder;

			// Yes, you could do this using String.Join, but it seems a bit wasteful to allocate all those "key=value" strings only to throw them away again.

			Action<StringBuilder, int> addQueryParameter = (builder, parameterIndex) =>
			{
				string parameterName = parameters.GetKey(parameterIndex);
				string parameterValue = parameters.Get(parameterIndex);

				builder.Append(parameterName);
				builder.Append('=');
				builder.Append(
					Uri.EscapeUriString(parameterValue)
				);
			};

			StringBuilder queryBuilder = new StringBuilder();
			
			// First parameter has no prefix.
			addQueryParameter(queryBuilder, 0);

			// Subsequent parameters are separated with an '&'
			for (int parameterIndex = 1; parameterIndex < parameters.Count; parameterIndex++)
			{
				queryBuilder.Append('&');
				addQueryParameter(queryBuilder, parameterIndex);
			}

			uriBuilder.Query = queryBuilder.ToString();

			return uriBuilder;
		}
	}
}
