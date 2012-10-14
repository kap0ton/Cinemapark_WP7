using System.Collections.Generic;

namespace Cinemapark.Model
{
	public class Schedule
	{
		public const string SchedultUri = "http://www.cinemapark.ru/gadgets/data/movie_schedule/{0}/{1}/"; //{0} - multiplex id, {1} - movie id

		public string Date { get; set; }

		public List<Hall> Halls { get; set; }
	}

	public class Hall
	{
		public int HallId { get; set; }

		public string HallName { get { return "hall " + HallId; } }

		public List<Session> Sessions { get; set; }
	}

	public class Session
	{
		public const string Booking = "http://www.cinemapark.ru/booking-v2/start/{0}/{1}/"; //{0} - multiplex id, {1} - session id
		public int Id { get; set; }

		public string Time { get; set; }
		
		public int Price { get; set; }
	}
}
