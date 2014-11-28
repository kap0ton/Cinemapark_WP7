using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Cinemapark.Lib.Entities;

namespace Cinemapark.Lib
{
	public class MovieService
	{
		public List<Multiplex> GetMultiplexes()
		{
			const string multiplexUri = "http://www.cinemapark.ru/gadgets/data/multiplexes/";
			var client = new WebClient();
			client.DownloadStringCompleted += GetDefaultMultiplexesCompleted;
			client.DownloadStringAsync(new Uri(multiplexUri, UriKind.Absolute));

			//todo: default implementation
			return new List<Multiplex>
			{
				new Multiplex
				{
					MultiplexId = 18,
					City = "Saratov",
					Title = "Triumph mall"
				}
			};
		}

		private void GetDefaultMultiplexesCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			var a = 1;
			//if (e.Error != null)
			//{
			//    Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
			//}
			//else
			//{
			//    TextReader textReader = new StringReader(e.Result);
			//    var xElement = XElement.Load(textReader);

			//    _items = (from item in xElement.Descendants("item")
			//        select new Multiplex
			//        {
			//            City = item.GetAttributeOrDefault("city"),
			//            Title = item.GetAttributeOrDefault("title"),
			//            MultiplexId = item.GetAttributeIntOrDefault("id")
			//        }).ToList().OrderBy(x => x.City).ThenBy(y => y.Title).ToList();

			if (_theEvent != null)
				_theEvent.Set();
			//}
		}

		private List<Multiplex> _items;
		AutoResetEvent _theEvent;

		public int GetItems()
		{
			_items = new List<Multiplex>();
			_theEvent = new AutoResetEvent(false);
			ThreadPool.QueueUserWorkItem(DoWork, _theEvent);

			if (_theEvent.WaitOne(10000))
			{
				return _items.Count;
			}

			return 0;
		}

		private void DoWork(object state)
		{
			const string multiplexUri = "http://www.cinemapark.ru/gadgets/data/multiplexes/";
			var client = new WebClient();
			client.DownloadStringCompleted += GetDefaultMultiplexesCompleted;
			client.DownloadStringAsync(new Uri(multiplexUri, UriKind.Absolute));

			//if (_theEvent != null)
			//    _theEvent.Set();

		}
	}
}
