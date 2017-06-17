﻿using UnityEngine;
using System.Xml;

namespace MyGame
{
	public static class String
	{
		public static string Get(uint stringId)
		{
			if (m_document == null)
			{
				LoadStrings();
			}

			string code = Key(stringId);
			XmlNode node = m_document.DocumentElement.SelectSingleNode(code);
			return node.InnerText;
		}

		private static XmlDocument m_document;

		private const char NODE_KEY = 's';

		private static void LoadStrings()
		{
			m_document = new XmlDocument();
			TextAsset xmlText = Resources.Load(GameData.locale) as TextAsset;
			m_document.LoadXml(xmlText.text);
		}
		private static string Key(uint stringId)
		{
			return NODE_KEY + stringId.ToString();
		}
	}
}
