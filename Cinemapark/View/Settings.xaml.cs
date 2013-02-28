using System;
using System.Windows;
using Cinemapark.Resources;
using Cinemapark.ViewModel;
using Microsoft.Phone.Shell;

namespace Cinemapark.View
{
	public partial class Settings
	{
		private readonly SettingsViewModel _settingsViewModel;

		public Settings()
		{
			InitializeComponent();
			_settingsViewModel = new SettingsViewModel();
			DataContext = _settingsViewModel;
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			_settingsViewModel.LoadMultiplexes();
		}

		private void SaveBarIconButton_OnClick(object sender, EventArgs e)
		{
			_settingsViewModel.SaveSelectedMultiplex();
			_settingsViewModel.SaveSelectedLanguage();
			NavigationService.GoBack();
		}

		private void Settings_OnLoaded(object sender, RoutedEventArgs e)
		{
			ApplicationBar = new ApplicationBar();

			var btnSave = new ApplicationBarIconButton(new Uri("/icons/appbar.save.rest.png", UriKind.Relative))
				{
					Text = AppResources.SaveBtn
				};
			btnSave.Click += SaveBarIconButton_OnClick;
			ApplicationBar.Buttons.Add(btnSave);
		}
	}
}
