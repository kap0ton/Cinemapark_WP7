using System;
using System.Windows;
using System.Windows.Controls;
using Cinemapark.Model;
using Cinemapark.ViewModel;
using Microsoft.Phone.Tasks;

namespace Cinemapark.View
{
	public partial class MovieDetails
	{
		private readonly MovieDetailsViewModel _movieDetailsViewModel;

		public MovieDetails()
		{
			InitializeComponent();

			_movieDetailsViewModel = new MovieDetailsViewModel();
			DataContext = _movieDetailsViewModel;
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			string idStr;
			if (NavigationContext.QueryString.TryGetValue("id", out idStr))
			{
				int id = int.Parse(idStr);
				_movieDetailsViewModel.Load(id);
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
