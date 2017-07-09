using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame.GameUtils
{
	using UpdaterPair = Pair<UpdaterEvent, EventDelegate>;
	using EventsList = List<Pair<UpdaterEvent, EventDelegate>>;

	public class Updater : MonoBehaviour
	{
		public void Add(UpdaterEvent eventToUpdate, UpdType type)
		{
			EventsList list = GetList(type);
			list.Add(UpdaterPair.Create(eventToUpdate, () => { }));
		}
		public void Add(UpdaterEvent eventToUpdate, EventDelegate onCompleteEvent, UpdType type)
		{
			EventsList list = GetList(type);
			list.Add(UpdaterPair.Create(eventToUpdate, onCompleteEvent));
		}
		public void Erase(UpdaterEvent eventToErase)
		{
			EraseFrom(m_fixedUpdEvents, eventToErase);
			EraseFrom(m_UpdEvents, eventToErase);
		}

		private void Awake()
		{
			m_fixedUpdEvents = new EventsList();
			m_UpdEvents = new EventsList();

			m_onRemove = new EventsList();
		}
		private void FixedUpdate()
		{
			Upd(m_fixedUpdEvents);
		}
		private void Update()
		{
			Upd(m_UpdEvents);
		}

		private void Upd(List<UpdaterPair> list)
		{
			list.RemoveAll(element =>
			{
				if (element == null)
				{
					return true;
				}

				m_onRemove.Add(element);
				bool isComplete = element.key();
				m_onRemove.Remove(element);
				if (isComplete) element.value();
				return isComplete;
			});
		}
		private EventsList GetList(UpdType type)
		{
			if (type == UpdType.FIXED)
			{
				return m_fixedUpdEvents;
			}
			else if (type == UpdType.UI)
			{
				return m_UpdEvents;
			}

			return null;
		}
		private void EraseFrom(EventsList list, UpdaterEvent eventToErase)
		{
			if (m_onRemove.Find(element => element.key == eventToErase) != null)
			{
				return;
			}

			list.RemoveAll(element => element.key == eventToErase);
		}

		private EventsList m_fixedUpdEvents;
		private EventsList m_UpdEvents;

		private EventsList m_onRemove;
	}

	public delegate bool UpdaterEvent();
}
