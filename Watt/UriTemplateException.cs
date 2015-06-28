using JetBrains.Annotations;
using System;
using System.Runtime.Serialization;

namespace DD.Cloud.WebApi.TemplateToolkit
{
	/// <summary>
	///		Exception raised when a <see cref="UriTemplate"/> is invalid or is missing required information.
	/// </summary>
	[Serializable]
	public class UriTemplateException
		: Exception
	{
		/// <summary>
		///		Create a new <see cref="UriTemplateException"/>.
		/// </summary>
		/// <param name="messageOrFormat">
		///		The exception message or message-format specifier.
		/// </param>
		/// <param name="formatArguments">
		///		Optional message format arguments.
		/// </param>
		[StringFormatMethod("messageOrFormat")]
		public UriTemplateException(string messageOrFormat, params object[] formatArguments)
			: base(String.Format(messageOrFormat, formatArguments))
		{
		}

		/// <summary>
		///		Create a new <see cref="UriTemplateException"/>.
		/// </summary>
		/// <param name="innerException">
		///		The exception that caused this exception to be raised.
		/// </param>
		/// <param name="messageOrFormat">
		///		The exception message or message-format specifier.
		/// </param>
		/// <param name="formatArguments">
		///		Optional message format arguments.
		/// </param>
		[StringFormatMethod("messageOrFormat")]
		public UriTemplateException(Exception innerException, string messageOrFormat, params object[] formatArguments)
			: base(String.Format(messageOrFormat, formatArguments), innerException)
		{
		}

		/// <summary>
		///		Serialisation constructor.
		/// </summary>
		/// <param name="info">
		///		A <see cref="SerializationInfo"/> that provides access to the serialised object data.
		/// </param>
		/// <param name="context">
		///		A <see cref="StreamingContext"/> structure containing information about the source of the serialised object data.
		/// </param>
		protected UriTemplateException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
