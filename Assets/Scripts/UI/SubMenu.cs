using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubMenu : MonoBehaviour
{
	public GameObject rootMenu;
	public GameObject selectedButton;

	private GameObject selectedButtonWhenReturning = null;

	public virtual void Open()
	{
		gameObject.SetActive(true);
		rootMenu.gameObject.SetActive(false);
		selectedButtonWhenReturning = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(selectedButton);
	}

	public virtual void Close()
	{
		gameObject.SetActive(false);
		rootMenu.gameObject.SetActive(true);
		EventSystem.current.SetSelectedGameObject(selectedButtonWhenReturning);
	}
}
