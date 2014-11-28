using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using Cinemapark.Annotations;
using Cinemapark.Model;
using Cinemapark.Resources;

namespace Cinemapark.ViewModel
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		#region Properties

		private Multiplex _multiplex;
		public Multiplex Multiplex
		{
			get { return _multiplex; }
			set
			{
				_multiplex = value;
				OnPropertyChanged("Multiplex");
			}
		}

		private readonly ObservableCollection<Movie> _movies;
		public ObservableCollection<Movie> Movies
		{
			get { return _movies; }
		}

		private readonly ObservableCollection<Movie> _theatreHDMovies;
		public ObservableCollection<Movie> TheatreHDMovies
		{
			get { return _theatreHDMovies; }
		}

		private bool _progressBarIsIndeterminate;
		public bool ProgressBarIsIndeterminate
		{
			get { return _progressBarIsIndeterminate; }
			set
			{
				_progressBarIsIndeterminate = value;
				OnPropertyChanged("ProgressBarIsIndeterminate");
			}
		}

		private Visibility _progressBarVisibility;
		public Visibility ProgressBarVisibility
		{
			get { return _progressBarVisibility; }
			set
			{
				_progressBarVisibility = value;
				OnPropertyChanged("ProgressBarVisibility");
			}
		}

		private readonly AppSettings _appSettings;

		private bool _isLoaded;

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region .ctor

		public MainPageViewModel()
		{
			_appSettings = new AppSettings();
			_movies = new ObservableCollection<Movie>();
			_theatreHDMovies = new ObservableCollection<Movie>();
			ProgressBarIsIndeterminate = false;
			ProgressBarVisibility = Visibility.Collapsed;
		}

		#endregion

		#region Load

		public void Load()
		{
			UpdateProgressBar(true);
			SetMultiplex();
			SetLanguage();
		}

		private void SetMultiplex()
		{
			Multiplex = _appSettings.Multiplex;
			if (Multiplex == null)
			{
				var client = new WebClient();
				client.DownloadStringCompleted += GetDefaultMultiplexesCompleted;
				client.DownloadStringAsync(new Uri(Multiplex.MultiplexUri, UriKind.Absolute));
			}
			else
			{
				LoadMovies();

				var s = new Lib.MovieService();
				var items = s.GetItems();
				Debug.Assert(items > 0);
			}
		}

		private void GetDefaultMultiplexesCompleted(object sender, DownloadStringCompletedEventArgs e)
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

					var items = (from item in xElement.Descendants("item")
					             select new Multiplex
						             {
							             City = item.GetAttributeOrDefault("city"),
							             Title = item.GetAttributeOrDefault("title"),
							             MultiplexId = item.GetAttributeIntOrDefault("id")
						             }).ToList().OrderBy(x => x.City).ThenBy(y => y.Title);

					var m = items.FirstOrDefault(x => x.MultiplexId == Multiplex.DefaultMultiplexId);
					Multiplex = m ?? items.FirstOrDefault();
					_appSettings.Multiplex = Multiplex;

					LoadMovies();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void SetLanguage()
		{
			//set language
			var language = _appSettings.Language;
			if (language == Language.None)
			{
				language = Thread.CurrentThread.CurrentCulture.Name == "ru-RU"
					? Language.Russian
					: Language.English;
				_appSettings.Language = language;
			}
			string culture;
			switch (language)
			{
				case Language.Russian:
					culture = "ru-RU";
					break;
				default:
					culture = "en-NZ";
					break;
			}
			var ci = new CultureInfo(culture);
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;
			((LocalizedStrings)Application.Current.Resources["LocalizedStrings"]).ResetResources();
		}

		#endregion

		#region Load Movies

		public void LoadMovies()
		{
			if (!_isLoaded)
			{
				UpdateProgressBar(true);
				Movies.Clear();
				TheatreHDMovies.Clear();
				var client = new WebClient();
				client.DownloadStringCompleted += GetMoviesCompleted;
				var path = string.Format(Movie.MoviesUri, _appSettings.Multiplex.MultiplexId);
				client.DownloadStringAsync(new Uri(path, UriKind.Absolute));
			}
			else
				UpdateProgressBar(false);
		}

		private void GetMoviesCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			try
			{
				if (e.Error != null)
				{
					Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
				}
				else
				{
					var multiplexId = _appSettings.Multiplex.MultiplexId;

					TextReader textReader = new StringReader(e.Result);
					var xElement = XElement.Load(textReader);

					var items = (from item in xElement.Descendants("item")
					             select new Movie
						             {
							             Title = item.GetAttributeOrDefault("title"),
							             MovieId = item.GetAttributeIntOrDefault("id"),
							             MultiplexId = multiplexId
						             }).ToList();

					foreach (var movie in items)
					{
						if (movie.Title.StartsWith("TheatreHD"))
							TheatreHDMovies.Add(movie);
						else
							Movies.Add(movie);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				UpdateProgressBar(false);
				_isLoaded = true;
			}
		}

		#endregion

		#region Helper Methods

		private void UpdateProgressBar(bool isEnabled)
		{
			if (isEnabled)
			{
				ProgressBarIsIndeterminate = true;
				ProgressBarVisibility = Visibility.Visible;
			}
			else
			{
				ProgressBarIsIndeterminate = false;
				ProgressBarVisibility = Visibility.Collapsed;
			}
		}

		#endregion
	}
}
