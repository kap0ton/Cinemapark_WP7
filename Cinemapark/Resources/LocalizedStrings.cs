using System.ComponentModel;
using Cinemapark.Annotations;

namespace Cinemapark.Resources
{
	public class LocalizedStrings : INotifyPropertyChanged
	{
		private readonly AppResources _localizedResourses;

		public AppResources AppResources
		{
			get { return _localizedResourses; }
		}

		public LocalizedStrings()
		{
			_localizedResourses = new AppResources();
		}

		public void ResetResources()
		{
			OnPropertyChanged("AppResources");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
