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
	public class SettingsViewModel : INotifyPropertyChanged
	{
		#region Properties

		private Multiplex _selectedMuliplex;
		public Multiplex SelectedMultiplex
		{
			get { return _selectedMuliplex; }
			set
			{
				_selectedMuliplex = value;
				OnPropertyChanged("SelectedMultiplex");
			}
		}

		private Language _selectedLanguage;
		public Language SelectedLanguage
		{
			get { return _selectedLanguage; }
			set
			{
				_selectedLanguage = value;
				OnPropertyChanged("SelectedLanguage");
			}
		}

		private readonly ObservableCollection<Multiplex> _multiplexes;
		public ObservableCollection<Multiplex> Multiplexes
		{
			get { return _multiplexes; }
		}

		private readonly AppSettings _appSettings;

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

		public SettingsViewModel()
		{
			_appSettings = new AppSettings();
			_multiplexes = new ObservableCollection<Multiplex>();
		}

		#endregion

		#region Load Multiplexes

		public void LoadMultiplexes()
		{
			var client = new WebClient();
			client.DownloadStringCompleted += GetMultiplexesCompleted;
			client.DownloadStringAsync(new Uri(Multiplex.MultiplexUri, UriKind.Absolute));
			SelectedLanguage = _appSettings.Language;
		}

		private void GetMultiplexesCompleted(object sender, DownloadStringCompletedEventArgs e)
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

					Multiplexes.Clear();
					foreach (var multiplex in items)
					{
						Multiplexes.Add(multiplex);
					}
					SetSelectedMultiplex();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void SetSelectedMultiplex()
		{
			if (_appSettings.Multiplex != null)
			{
				SelectedMultiplex = Multiplexes.FirstOrDefault(p => p.MultiplexId == _appSettings.Multiplex.MultiplexId);
			}
		}

		#endregion

		#region Save

		public void SaveSelectedMultiplex()
		{
			_appSettings.Multiplex = SelectedMultiplex;
		}

		public void SaveSelectedLanguage()
		{
			_appSettings.Language = SelectedLanguage;
		}

		#endregion
	}
}
