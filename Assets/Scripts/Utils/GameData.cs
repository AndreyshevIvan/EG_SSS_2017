using UnityEngine;

namespace MyGame
{
	public class GameData
	{
		public static string locale
		{
			get
			{
				string localeName = PlayerPrefs.GetString(LOCALE_KEY, "eng");
				return LOCALE_PATH + localeName + LOCALE_FILE_NAME;
			}
		}

		private const string LOCALE_FILE_NAME = "_locale";
		private const string LOCALE_PATH = "locales/";
		private const string LOCALE_KEY = "locale";
	}

}
