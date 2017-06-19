using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyGame
{
	public class ShipController
		: MonoBehaviour
		, IDragHandler
		, IBeginDragHandler
	{
		public GameObject m_ship;
		public GameplayController m_gameplay;

		public void OnBeginDrag(PointerEventData eventData)
		{
			Debug.Log("OnBeginDrag");
		}

		public void OnDrag(PointerEventData eventData)
		{
			Debug.Log("OnDrag");
		}

		private void Awake()
		{

		} 
		private void Start()
		{

		}
	}
}
