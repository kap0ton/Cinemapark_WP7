using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Cinemapark.Model;
using Cinemapark.ViewModel;
using Microsoft.Phone.Tasks;

namespace Cinemapark.View
{
	public partial class MovieDetailsView
	{
		private readonly MovieDetailsViewModel _movieDetailsViewModel;

		public MovieDetailsView()
		{
			InitializeComponent();

			_movieDetailsViewModel = new MovieDetailsViewModel();
			DataContext = _movieDetailsViewModel;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			string idStr;
			string titleStr;
			if (NavigationContext.QueryString.TryGetValue("id", out idStr)
				&& NavigationContext.QueryString.TryGetValue("title", out titleStr))
			{
				var id = int.Parse(idStr);
				var title = HttpUtility.UrlDecode(titleStr);
				_movieDetailsViewModel.Load(id, title);
			}
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			var hyperlynk = sender as HyperlinkButton;
			if (hyperlynk != null)
			{
				var task = new WebBrowserTask
					{
						Uri =
							new Uri(string.Format(Session.BookingUrl, _movieDetailsViewModel.Multiplex.MultiplexId, hyperlynk.Tag), UriKind.Absolute)
					};
				task.Show();
			}
		}
	}
}
