using System.Windows;

namespace Cinemapark.Model
{
	public class MovieDetails : Movie
	{
		#region Properties

		/// <summary>
		/// {0} - Movie title
		/// </summary>
		public const string MovieDetailsUri = "http://kinoinfo.ru/api/film/?name={0}";

		private string _titleOriginal;
		public string TitleOriginal
		{
			get { return _titleOriginal; }
			set
			{
				_titleOriginal = value;
				OnPropertyChanged("TitleOriginal");
			}
		}

		private string _description;
		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;
				OnPropertyChanged("Description");
			}
		}

		private string _genres;
		public string Genres
		{
			get { return _genres; }
			set
			{
				_genres = value;
				OnPropertyChanged("Genres");
			}
		}

		private string _actors;
		public string Actors
		{
			get { return _actors; }
			set
			{
				_actors = value;
				OnPropertyChanged("Actors");
			}
		}

		private string _duration;
		public string Duration
		{
			get { return _duration; }
			set
			{
				_duration = value;
				OnPropertyChanged("Duration");
			}
		}

		private bool _noData;
		public bool NoData
		{
			get { return _noData; }
			set
			{
				_noData = value;
				OnPropertyChanged("NoData");
				OnPropertyChanged("SelectedTemplate");
			}
		}

		public DataTemplate SelectedTemplate
		{
			get
			{
				if (NoData)
					return Application.Current.Resources["DataNotFoundDataTemplate"] as DataTemplate;
				return Application.Current.Resources["MovieDetailsDataTemplate"] as DataTemplate;
			}
		}

		#endregion

		#region .ctor

		public MovieDetails()
		{
			Duration = "0";
		}

		#endregion
	}
}
