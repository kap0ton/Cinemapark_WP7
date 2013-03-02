using System.ComponentModel;
using Cinemapark.Annotations;

namespace Cinemapark.Model
{
	public class MovieDetails : INotifyPropertyChanged
	{
		/// <summary>
		/// {0} - Movie title
		/// </summary>
		public const string MovieDetailsUri = "http://kinoinfo.ru/api/film/?name={0}";

		private string _title;
		public string Title
		{
			get { return _title; }
			set
			{
				_title = value;
				OnPropertyChanged("Title");
			}
		}

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

		public MovieDetails()
		{
			Duration = "0";
		}

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
