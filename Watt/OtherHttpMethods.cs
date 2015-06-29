using System.Net.Http;

namespace DD.Cloud.WebApi.TemplateToolkit
{
	/// <summary>
	///		Additional standard HTTP methods.
	/// </summary>
	public static class OtherHttpMethods
	{
		/// <summary>
		///		The HTTP PATCH method.
		/// </summary>
		public static readonly HttpMethod Patch = new HttpMethod("PATCH");
	}
}
