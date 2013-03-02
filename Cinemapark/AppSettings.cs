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

		#region Helper methods

		private bool AddOrUpdateValue(string key, object value)
		{
			var valueChanged = false;

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

		#endregion

		#region Multiplex

		private const string MultiplexKey = "MultiplexKey";

		/// <summary>
		/// Gets or sets Multiplex
		/// </summary>
		public Multiplex Multiplex
		{
			get { return GetValueOrDefault<Multiplex>(MultiplexKey, null); }
			set
			{
				if (AddOrUpdateValue(MultiplexKey, value))
					Save();
			}
		}

		#endregion

		#region Language

		private const string LanguageKey = "LanguageKey";

		/// <summary>
		/// Gets or sets Language
		/// </summary>
		public Language Language
		{
			get { return GetValueOrDefault(LanguageKey, Language.English); }
			set
			{
				if(AddOrUpdateValue(LanguageKey, value))
					Save();
			}
		}

		#endregion
	}
}
