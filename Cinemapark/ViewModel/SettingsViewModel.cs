﻿using System;
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
				NotifyPropertyChanged("SelectedMultiplex");
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

		public SettingsViewModel()
		{
			_appSettings = new AppSettings();
			_multiplexes = new ObservableCollection<Multiplex>();
		}

		#endregion

		public void LoadMultiplexes()
		{
			var client = new WebClient();
			client.DownloadStringCompleted += GetMultiplexesCompleted;
			client.DownloadStringAsync(new Uri(Multiplex.MultiplexUri, UriKind.Absolute));
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
					XDocument xDocument = XDocument.Load(textReader);

					var items = (from item in xDocument.Root.Descendants("item")
					             select new Multiplex
					                    	{
					                    		City = item.Attribute("city").Value,
					                    		Title = item.Attribute("title").Value,
					                    		MultiplexId = Int32.Parse(item.Attribute("id").Value)
					                    	}).ToList().OrderBy(x=>x.City).ThenBy(y=>y.Title);

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

		public void SaveSelectedMultiplex()
		{
			_appSettings.Multiplex = SelectedMultiplex;
		}
	}
}
