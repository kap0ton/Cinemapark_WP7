using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Xml.Linq;
using Cinemapark.Annotations;
using Cinemapark.Model;

namespace Cinemapark.ViewModel
{
	public class MovieDetailsViewModel : INotifyPropertyChanged
	{
		#region Properties

		private Multiplex _multiplex;
		public Multiplex Multiplex
		{
			get { return _multiplex; }
			set
			{
				_multiplex = value;
				NotifyPropertyChanged("Multiplex");
			}
		}

		private readonly ObservableCollection<Schedule> _schedules;
		public ObservableCollection<Schedule> Schedules
		{
			get { return _schedules; }
		}

		private Uri _imageUrl;
		public Uri ImageUrl
		{
			get { return _imageUrl; }
			set
			{
				_imageUrl = value;
				NotifyPropertyChanged("ImageUrl");
			}
		}

		private MovieDetails _movieDetails;
		public MovieDetails MovieDetails
		{
			get { return _movieDetails; }
			set
			{
				_movieDetails = value;
				NotifyPropertyChanged("MovieDetails");
			}
		}

		private readonly AppSettings _appSettings;

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void NotifyPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region .ctor

		public MovieDetailsViewModel()
		{
			_appSettings = new AppSettings();
			_schedules = new ObservableCollection<Schedule>();
		}

		#endregion

		#region Load

		public void Load(int movieId, string title)
		{
			Multiplex = _appSettings.Multiplex;
			MovieDetails = new MovieDetails {MovieId = movieId, Title = title};
			GetKinopoiskDetails();
			LoadSchedule(movieId);
		}

		#endregion

		#region Load Movie Details

		private void GetKinopoiskDetails()
		{
			var client = new WebClient();
			client.DownloadStringCompleted += GetKinopoiskDetailsCompleted;
			var path = string.Format(MovieDetails.MovieDetailsUri, HttpUtility.UrlEncode(MovieDetails.Title));
			client.DownloadStringAsync(new Uri(path, UriKind.Absolute));
		}

		private void GetKinopoiskDetailsCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			try
			{
				if (e.Error != null)
				{
					Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
				}
				else
				{
					TextReader textReader = new StringReader(e.Result);
					var xDocument = XElement.Load(textReader);

					var movie = xDocument.Descendants("film").FirstOrDefault();
					if (movie != null)
					{
						MovieDetails.TitleOriginal = HttpUtility.HtmlDecode(movie.GetAttributeOrDefault("original"));

						var duration = movie.Elements().FirstOrDefault(x => x.Name == "runtime");
						if (duration != null)
						{
							MovieDetails.Duration = duration.GetAttributeOrDefault("value");
						}

						var genres = movie.Elements().FirstOrDefault(x => x.Name == "genres");
						if (genres != null && genres.Elements().Any())
						{
							var res = genres.Elements().Select(x => x.GetAttributeOrDefault("name")).Aggregate((x, y) => x + ", " + y);
							MovieDetails.Genres = res;
						}

						var actors = movie.Elements().FirstOrDefault(x => x.Name == "persons");
						if (actors != null && actors.Elements().Any())
						{
							MovieDetails.Actors = actors.Elements().Select(x => x.GetAttributeOrDefault("name")).Aggregate((x, y) => x + ", " + y);
						}

						var desc = movie.Elements().FirstOrDefault(x => x.Name == "description");
						if (desc != null)
						{
							MovieDetails.Description = desc.GetAttributeOrDefault("value");
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		#endregion

		#region Load Schedule

		private void LoadSchedule(int movieId)
		{
			var client = new WebClient();
			client.DownloadStringCompleted += GetSchedultCompleted;
			var path = string.Format(Schedule.SchedultUri, _multiplex.MultiplexId, movieId);
			client.DownloadStringAsync(new Uri(path));
		}

		private void GetSchedultCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			try
			{
				if (e.Error != null)
				{
					Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
				}
				else
				{
					TextReader textReader = new StringReader(e.Result);
					var xElement = XElement.Load(textReader);

					var items = (from item in xElement.Descendants("date")
					             select new Schedule
						             {
							             Date = item.GetAttributeOrDefault("name"),
							             Halls = (from hall in item.Elements("hall")
							                      select new Hall
								                      {
									                      HallId = hall.GetAttributeIntOrDefault("id"),
									                      Sessions = (from session in hall.Elements("item")
									                                  select new Session
										                                  {
											                                  Id = session.GetAttributeIntOrDefault("id"),
											                                  Time = session.GetAttributeOrDefault("time"),
											                                  Price = session.GetAttributeIntOrDefault("price")
										                                  }).ToList()
								                      }).ToList()
						             }).ToList();

					Schedules.Clear();
					foreach (var schedule in items)
					{
						Schedules.Add(schedule);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		#endregion
	}
}
