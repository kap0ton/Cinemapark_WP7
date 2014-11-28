namespace Cinemapark.Lib.Entities
{
	public class Multiplex
	{
		public int MultiplexId { get; set; }

		public string City { get; set; }

		public string Title { get; set; }

		public string FullName
		{
			get { return City + ", " + Title; }
		}
	}
}
