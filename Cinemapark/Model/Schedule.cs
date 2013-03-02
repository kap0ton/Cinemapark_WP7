using System.Collections.Generic;

namespace Cinemapark.Model
{
	public class Schedule
	{
		/// <summary>
		/// {0} - multiplex id, {1} - movie id
		/// </summary>
		public const string SchedultUri = "http://www.cinemapark.ru/gadgets/data/movie_schedule/{0}/{1}/";

		public string Date { get; set; }

		public List<Hall> Halls { get; set; }
	}

	public class Hall
	{
		public int HallId { get; set; }

		public List<Session> Sessions { get; set; }
	}

	public class Session
	{
		/// <summary>
		/// {0} - multiplex id, {1} - session id
		/// </summary>
		public const string BookingUrl = "http://booking.www.cinemapark.ru/start/{0}/{1}/";

		public int Id { get; set; }

		public string Time { get; set; }

		public int Price { get; set; }
	}
}
