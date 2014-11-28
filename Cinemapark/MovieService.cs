using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using Cinemapark.Model;
using Cinemapark.ViewModel;

namespace Cinemapark
{
	public class LoadMoviesEventArgs : EventArgs
	{
		public int MultiplexId { get; set; }
	}

	public class MovieService
	{
		private readonly List<Movie> _movies;
		public List<Movie> Movies
		{
			get { return _movies; }
		}

		private readonly int _multiplexId;

		public delegate void LoadMoviesCompletedEventHandler(LoadMoviesEventArgs args);
		public event LoadMoviesCompletedEventHandler OnLoadMovies;

		public MovieService(int multiplexId)
		{
			_multiplexId = multiplexId;

			_movies = new List<Movie>();
		}

		//public void LoadMovies()
		//{
		//    Thread.Sleep(5000);

		//    var handler = OnLoadMovies;
		//    if (handler != null)
		//        handler(new LoadMoviesEventArgs {MultiplexId = _multiplexId});
		//}



		public void LoadMovies()
		{
			var client = new WebClient();
			client.DownloadStringCompleted += GetMoviesCompleted;
			var path = string.Format(Movie.MoviesUri, _multiplexId);
			client.DownloadStringAsync(new Uri(path, UriKind.Absolute));
		}

		private void GetMoviesCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			try
			{
				if (e.Error != null)
				{
					//Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
					throw e.Error;
				}
				else
				{
					Movies.Clear();
					//TheatreHDMovies.Clear();

					TextReader textReader = new StringReader(e.Result);
					var xElement = XElement.Load(textReader);

					var items = (from item in xElement.Descendants("item")
						select new Movie
						{
							Title = item.GetAttributeOrDefault("title"),
							MovieId = item.GetAttributeIntOrDefault("id"),
							MultiplexId = _multiplexId
						}).ToList();

					foreach (var movie in items)
					{
						Movies.Add(movie);
					}
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
				throw;
			}
			finally
			{
			}
		}
	}


	public class TheMovie
	{
		private int id;
		private string title;
	}


	public class TheSession
	{
		private int movieId;

		private string date;
		private int hallId;
		private int id;
		private string time;
		private decimal price;
	}
}
