namespace Cinemapark.Model
{
	public class Multiplex
	{
		/// <summary>
		/// 18 - Saratov, Triumph Mall
		/// </summary>
		public const int DefaultMultiplexId = 18;

		public const string MultiplexUri = "http://www.cinemapark.ru/gadgets/data/multiplexes/";

		public int MultiplexId { get; set; }

		public string City { get; set; }
	
		public string Title { get; set; }

		public string FullName
		{
			get { return City + ", " + Title; }
		}
	}
}
