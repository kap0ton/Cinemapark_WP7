using System.IO.IsolatedStorage;
using Cinemapark.Model;

namespace Cinemapark
{
	public class AppSettings
	{
		private readonly IsolatedStorageSettings _settings;

		public AppSettings()
		{
			_settings = IsolatedStorageSettings.ApplicationSettings;
		}

		private bool AddOrUpdateValue(string key, object value)
		{
			bool valueChanged = false;

			if (_settings.Contains(key))
			{
				if (_settings[key] != value)
				{
					_settings[key] = value;
					valueChanged = true;
				}
			}
			else
			{
				_settings.Add(key, value);
				valueChanged = true;
			}
			return valueChanged;
		}

		private T GetValueOrDefault<T>(string key, T defaultValue)
		{
			T value;

			if (_settings.Contains(key))
			{
				value = (T)_settings[key];
			}
			else
			{
				value = defaultValue;
			}
			return value;
		}

		private void Save()
		{
			_settings.Save();
		}

		private const string MultiplexKey = "MultiplexKey";

		public Multiplex Multiplex
		{
			get { return GetValueOrDefault<Multiplex>(MultiplexKey, null); }
			set
			{
				if (AddOrUpdateValue(MultiplexKey, value))
					Save();
			}
		}
	}
}
