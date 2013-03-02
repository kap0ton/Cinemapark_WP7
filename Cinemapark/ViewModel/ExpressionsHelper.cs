using System.Xml.Linq;

namespace Cinemapark.ViewModel
{
	public static class ExpressionsHelper
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
	}
}
