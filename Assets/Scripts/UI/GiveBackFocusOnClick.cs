using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GiveBackFocusOnClick : MonoBehaviour, IPointerDownHandler
{
	GameObject lastSelected;

	private void Update()
	{
		GameObject currentlySelected = EventSystem.current.currentSelectedGameObject;
		if (currentlySelected != this.gameObject)
		{
			lastSelected = currentlySelected;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		EventSystem.current.SetSelectedGameObject(lastSelected);
	}
}
