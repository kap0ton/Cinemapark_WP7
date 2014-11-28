using System.Xml.Linq;

namespace Cinemapark.Lib
{
	public static class ExpressionHelper
	{
		public static string GetAttributeOrDefault(this XElement element, string attributeName)
		{
			var attr = element.Attribute(attributeName);
			return attr != null ? attr.Value : string.Empty;
		}

		public static int GetAttributeIntOrDefault(this XElement element, string attributeName)
		{
			var attr = element.Attribute(attributeName);
			if (attr != null)
			{
				int res;
				if (int.TryParse(attr.Value, out res))
					return res;
			}
			return 0;
		}

		public static string TrimMovieTitle(this string source)
		{
			return source.Replace(" IMAX 2D", "").Replace(" 2D", "").Replace(" IMAX 3D", "").Replace(" 3D", "").Trim();
		}
	}
}
