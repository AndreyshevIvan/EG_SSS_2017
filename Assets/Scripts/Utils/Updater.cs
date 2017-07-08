using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame.GameUtils
{
	using UpdaterPair = Pair<UpdaterEvent, EventDelegate>;

	public class Updater : MonoBehaviour
	{
		public void Add(UpdaterEvent eventToUpdate)
		{
			m_events.Add(UpdaterPair.Create(eventToUpdate, () => { }));
		}
		public void Add(UpdaterEvent eventToUpdate, EventDelegate onCompleteEvent)
		{
			m_events.Add(UpdaterPair.Create(eventToUpdate, onCompleteEvent));
		}
		public void Erase(UpdaterEvent eventToErase)
		{
			if (m_onUpdate.Find(element => element.key == eventToErase) != null)
			{
				return;
			}

			m_events.RemoveAll(element => element.key == eventToErase);
		}

		private void Awake()
		{
			m_events = new List<UpdaterPair>();
			m_onUpdate = new List<UpdaterPair>();
		}
		private void FixedUpdate()
		{
			m_events.RemoveAll(element =>
			{
				if (element == null)
				{
					return true;
				}

				m_onUpdate.Add(element);
				bool isComplete = element.key();
				m_onUpdate.Remove(element);
				if (isComplete) element.value();
				return isComplete;
			});
		}

		private List<UpdaterPair> m_events;
		private List<UpdaterPair> m_onUpdate;
	}

	public delegate bool UpdaterEvent();
}
