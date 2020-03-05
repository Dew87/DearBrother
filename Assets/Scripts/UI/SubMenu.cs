using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubMenu : MonoBehaviour
{
	public GameObject rootMenu;
	public PauseMenu pauseMenu;
	public GameObject selectedButton;

	private GameObject selectedButtonWhenReturning = null;

	public virtual void Open()
	{
		gameObject.SetActive(true);
		rootMenu.gameObject.SetActive(false);
		selectedButtonWhenReturning = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(selectedButton);
		pauseMenu.isInSubMenu = true;
	}

	public virtual void Close()
	{
		Debug.Log("close sub");
		gameObject.SetActive(false);
		rootMenu.gameObject.SetActive(true);
		EventSystem.current.SetSelectedGameObject(selectedButtonWhenReturning);
		pauseMenu.isInSubMenu = false;
	}

	private void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			Close();
		}
	}
}
