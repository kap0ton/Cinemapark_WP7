using System;

namespace Cinemapark.Model
{
	public class Movie
	{
		/// <summary>
		/// {0} - multiplex id
		/// </summary>
		public const string MoviesUri = "http://www.cinemapark.ru/gadgets/data/movies/{0}/";

		//private const string PosterUri = "http://www.cinemapark.ru/img/poster_large/{0}.jpg"; //{0} - movie id

		/// <summary>
		/// {0} - movie id
		/// </summary>
		public const string PosterUri = "http://stasis.www.cinemapark.ru/img/film/poster_large/{0}.jpg";

		public string Title { get; set; }

		public int MovieId { get; set; }

		public int MultiplexId { get; set; }

		public Uri ImageUrl
		{
			get { return new Uri(string.Format(PosterUri, MovieId), UriKind.Absolute); }
		}
	}
}
