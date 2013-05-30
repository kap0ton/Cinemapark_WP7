using System;
using System.Globalization;

namespace Cinemapark.Model
{
	public class Movie
	{
		/// <summary>
		/// {0} - multiplex id
		/// </summary>
		public const string MoviesUri = "http://www.cinemapark.ru/gadgets/data/movies/{0}/";

		/// <summary>
		///  //{0} - multiplex id, {1} - booking numper
		/// </summary>
		public const string BookinInfoUri = "http://booking.www.cinemapark.ru/info/18/449355/";

		/// <summary>
		/// {0} - movie id
		/// </summary>
		public const string PosterUri = "http://stasis.www.cinemapark.ru/img/film/poster_large/{0}.jpg";

		public string Title { get; set; }

		public int MovieId { get; set; }

		public int MultiplexId { get; set; }

		public Uri ImageUrl
		{
			get
			{
				return new Uri(
					string.Format(PosterUri, MovieId.ToString(CultureInfo.InvariantCulture).Substring(0, 4)),
					UriKind.Absolute);
			}
		}
	}
}
