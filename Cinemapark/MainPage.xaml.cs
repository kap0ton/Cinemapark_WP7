﻿using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using Cinemapark.Model;
using Cinemapark.ViewModel;

namespace Cinemapark
{
	public partial class MainPage
	{
		private readonly MainPageViewModel _mainPageViewModel;

		// Constructor
		public MainPage()
		{
			InitializeComponent();

			_mainPageViewModel = new MainPageViewModel();
			DataContext = _mainPageViewModel;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			_mainPageViewModel.Load();
		}

		private void RefreshBarIconButton_OnClick(object sender, EventArgs e)
		{
			_mainPageViewModel.LoadMovies();
		}

		private void SettingsBarIconButton_OnClick(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/View/Settings.xaml", UriKind.Relative));
		}

		private void MovieListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var movie = MovieListBox.SelectedItem as Movie;
			if(movie!=null)
			{
				NavigationService.Navigate(new Uri("/View/MovieDetails.xaml?id=" + movie.MovieId, UriKind.Relative));
			}
		}

		//save results after reservation a place
	}
}
