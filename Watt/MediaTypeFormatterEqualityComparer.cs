using System.Collections.Generic;
using System.Net.Http.Formatting;

namespace DD.Cloud.WebApi.TemplateToolkit
{
	/// <summary>
	///		Equality comparer for media-type formatters.
	/// </summary>
	internal sealed class MediaTypeFormatterEqualityComparer
		: EqualityComparer<MediaTypeFormatter>
	{
		/// <summary>
		///		Media-type formatter equality comparer that only compares formatter type.
		/// </summary>
		public static readonly IEqualityComparer<MediaTypeFormatter> ByType = new MediaTypeFormatterEqualityComparer();

		/// <summary>
		///		Create a new media-type formatter equality comparer.
		/// </summary>
		MediaTypeFormatterEqualityComparer()
		{
		}

		/// <summary>
		///		Determine whether 2 media-type formatters are equal.
		/// </summary>
		/// <param name="firstFormatter">
		///		The first media-type formatter.
		/// </param>
		/// <param name="secondFormatter">
		///		The second media-type formatter.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the media-type formatters have the same type; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(MediaTypeFormatter firstFormatter, MediaTypeFormatter secondFormatter)
		{
			if (firstFormatter == null && secondFormatter == null)
				return true;

			if (firstFormatter == null || secondFormatter == null)
				return false;

			return firstFormatter.GetType() == secondFormatter.GetType();
		}

		/// <summary>
		///		Get a hash code for the media-type formatter.
		/// </summary>
		/// <param name="formatter">
		///		The formatter.
		/// </param>
		/// <returns>
		///		The hash code.
		/// </returns>
		public override int GetHashCode(MediaTypeFormatter formatter)
		{
			if (formatter == null)
				return 0;

			return formatter.GetType().GetHashCode();
		}
	}
}