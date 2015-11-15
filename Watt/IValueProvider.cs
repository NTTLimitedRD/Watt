namespace DD.Cloud.WebApi.TemplateToolkit
{
	/// <summary>
	///		Represents the provider for a value from an instance of <typeparamref name="TContext"/>.
	/// </summary>
	/// <typeparam name="TValue">
	///		The type of value extracted by the provider.
	/// </typeparam>
	/// <typeparam name="TContext">
	///		The source type from which the value is extracted.
	/// </typeparam>
	public interface IValueProvider<in TContext, out TValue>
	{
		/// <summary>
		///		Extract the value from the specified context.
		/// </summary>
		/// <param name="source">	
		///		The <typeparamref name="TContext"/> instance from which the value is to be extracted.
		/// </param>
		/// <returns>
		///		The value.
		/// </returns>
		TValue Get(TContext source);
	}
}
