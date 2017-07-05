using UnityEngine;
using System.Xml;
using System;
using System.ComponentModel;

namespace MyGame
{
	public static class StrManager
	{
		public static string Get(uint stringId)
		{
			if (m_reader == null)
			{
				m_reader = new MyXmlReader<string>(GameData.locale);
			}

			string[] path = { key + stringId.ToString() };
			return m_reader.GetInnerText(path);
		}

		private static MyXmlReader<string> m_reader;
		private static string key = "s";
	}

	public static class LevelManager
	{
		public static uint GetPrice(uint level)
		{
			if (m_reader == null)
			{
				m_reader = new MyXmlReader<uint>(GameData.LEVELS_PATH);
			}

			string[] path = { key + level.ToString() };
			return m_reader.GetInnerText(path);
		}

		private static MyXmlReader<uint> m_reader;
		private static string key = "level_";
	}

	public class MyXmlReader<T>
	{
		public MyXmlReader(string docName)
		{
			m_document = new XmlDocument();
			TextAsset xmlText = Resources.Load(docName) as TextAsset;
			m_document.LoadXml(xmlText.text);
			m_converter = TypeDescriptor.GetConverter(typeof(T));
		}

		public T GetInnerText(string[] path)
		{
			XmlNode node = m_document.DocumentElement.SelectSingleNode(path[0]);

			for (int i = 1; i < path.Length; i++)
			{
				node = node.SelectSingleNode(path[i]);
			}
			 
			return (T)m_converter.ConvertFromString(node.InnerText);
		}

		private XmlDocument m_document;
		private TypeConverter m_converter;

	}
}
