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
		public static byte minModLevel { get { return 0; } }
		public static byte maxModLevel { get { return 5; } }
		public static BoundingBox mapBox
		{
			get { return new BoundingBox(-10, 10, -25, 25); }
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
		public static uint GetNeededExp(ushort level)
		{
			return 1000;
		}
		public static void SaveShip(ShipProperties properties)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(SHIPS_PROPERTIES_PATH, FileMode.Create);
			formatter.Serialize(stream, properties);
			stream.Close();
		}
		public static ShipProperties LoadShip(ShipType type)
		{
			string file = SHIPS_PROPERTIES_PATH;

			if (!File.Exists(file))
			{
				ShipProperties newShip = new ShipProperties();
				SaveShip(newShip);
			}

			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(file, FileMode.Open);
			ShipProperties ship = (ShipProperties)formatter.Deserialize(stream);
			stream.Close();
			return ship;
		}

		static string LOCALE_FILE_NAME = "_locale";
		static string LOCALE_PATH = "locales/";
		static string LOCALE_KEY = "locale";
		static string RESOURCES_PATH = Application.dataPath + "/Resources/";
		static string FTYPE = ".txt";
		static string USER_FILE_NAME = "user" + FTYPE;
		static string USER_FILE = RESOURCES_PATH + USER_FILE_NAME;
		static string SHIPS_PROPERTIES_PATH = RESOURCES_PATH + "ShipProperties";
	}
}
