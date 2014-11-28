using Cinemapark.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cinemapark.Tests
{
	/// <summary>
	/// Summary description for MultiplexesUnitTest
	/// </summary>
	[TestClass]
	public class MultiplexesUnitTest
	{
		[TestMethod]
		public void GetMultiplexesTestMethod()
		{
			var service = new MovieService();
			var multiplexes = service.GetMultiplexes();
			Assert.IsTrue(multiplexes.Count > 0);
		}

		[TestMethod]
		public void GetItemsTestMethod()
		{
			var service = new MovieService();
			var res = service.GetItems();
			Assert.IsTrue(res > 0);

		}
	}
}
