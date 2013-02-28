using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
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
				NotifyPropertyChanged("Multiplex");
			}
		}

		private readonly ObservableCollection<Movie> _movies;
		public ObservableCollection<Movie> Movies
		{
			get { return _movies; }
		}

		private readonly AppSettings _appSettings;

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;
		
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

		public MainPageViewModel()
		{
			_appSettings = new AppSettings();
			_movies = new ObservableCollection<Movie>();
		}

		#endregion

		public void Load()
		{
			Multiplex = _appSettings.Multiplex;
			if (_multiplex == null)
			{
				//load default multiplex
				Multiplex = new Multiplex
					{
						MultiplexId = 18,
						City = "Саратов",
						Title = "Триумф Молл"
					};
				_appSettings.Multiplex = Multiplex;
			}
			string culture;
			switch (_appSettings.Language)
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
			((LocalizedStrings) Application.Current.Resources["LocalizedStrings"]).ResetResources();

			//load movies
			LoadMovies();
		}

		public void LoadMovies()
		{
			var client = new WebClient();
			client.DownloadStringCompleted += GetMoviesCompleted;
			var path = string.Format(Movie.MovieUri, _appSettings.Multiplex.MultiplexId);
			client.DownloadStringAsync(new Uri(path, UriKind.Absolute));
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
					XDocument xDocument = XDocument.Load(textReader);

					var items = (from item in xDocument.Root.Descendants("item")
					             select new Movie
					                    	{
					                    		Title = item.Attribute("title").Value,
					                    		MovieId = Int32.Parse(item.Attribute("id").Value),
					                    		MultiplexId = multiplexId
					                    	}).ToList();

					Movies.Clear();
					foreach (var movie in items)
					{
						Movies.Add(movie);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
