namespace IleriWeb.Web.Helpers
{
	public static class StringHelper
	{
		public static string TruncateString(string input, int maxLength)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			return input.Length <= maxLength ? input : input.Substring(0, maxLength) + "...";
		}
	}
}
