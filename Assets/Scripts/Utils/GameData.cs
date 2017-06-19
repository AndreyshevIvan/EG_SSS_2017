using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MyGame
{
	public static class GameData
	{
		public static string locale
		{
			get
			{
				string localeName = PlayerPrefs.GetString(LOCALE_KEY, "eng");
				return LOCALE_PATH + localeName + LOCALE_FILE_NAME;
			}
		}

		public static void SaveUser(User user)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(USER_FILE, FileMode.Create);
			formatter.Serialize(stream, user);
			stream.Close();
		}
		public static User LoadUser()
		{
			if (!File.Exists(USER_FILE))
			{
				User newUser = new User();
				SaveUser(newUser);
			}

			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(USER_FILE, FileMode.Open);
			User user = formatter.Deserialize(stream) as User;
			stream.Close();

			return user;
		}

		static string LOCALE_FILE_NAME = "_locale";
		static string LOCALE_PATH = "locales/";
		static string LOCALE_KEY = "locale";
		static string RESOURCES_PATH = Application.dataPath + "/Resources/";
		static string FILE_TYPE = ".txt";
		static string USER_FILE_NAME = "user" + FILE_TYPE;
		static string USER_FILE = RESOURCES_PATH + USER_FILE_NAME;

	}

}
