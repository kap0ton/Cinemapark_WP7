using System;
using Cinemapark.ViewModel;

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
			NavigationService.GoBack();
		}
	}
}
