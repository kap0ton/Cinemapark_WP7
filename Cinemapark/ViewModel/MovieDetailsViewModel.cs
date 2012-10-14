using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Xml.Linq;
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

		public MovieDetailsViewModel()
		{
			_appSettings = new AppSettings();
			_schedules = new ObservableCollection<Schedule>();
		}

		#endregion

		public void Load(int movieId)
		{
			Multiplex = _appSettings.Multiplex;
			ImageUrl = new Uri(string.Format(Movie.PosterUri, movieId), UriKind.Absolute);

			LoadSchedule(movieId);
		}

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
					XDocument xDocument = XDocument.Load(textReader);

					var items = (from item in xDocument.Root.Descendants("date")
					             select new Schedule
					                    	{
					                    		Date = item.Attribute("name").Value,
					                    		Halls = (from hall in item.Elements("hall")
					                    		         select new Hall
					                    		                	{
					                    		                		HallId = int.Parse(hall.Attribute("id").Value),
					                    		                		Sessions = (from session in hall.Elements("item")
					                    		                		            select new Session
					                    		                		                   	{
					                    		                		                   		Id = int.Parse(session.Attribute("id").Value),
					                    		                		                   		Time = session.Attribute("time").Value,
					                    		                		                   		Price = int.Parse(session.Attribute("price").Value)
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
	}
}
