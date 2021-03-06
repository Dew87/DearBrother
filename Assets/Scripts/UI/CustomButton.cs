﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, ISubmitHandler, ISelectHandler, IDeselectHandler
{
	public Text text;
	public GameObject selectIndicatorPosition;

	[HideInInspector] public SubMenu menu;

	private Color normalColor;

	private void Start()
	{
		if (text)
		{
			normalColor = text.color; 
		}
		SubMenu[] menus = GetComponentsInParent<SubMenu>(true);
		menu = menus.Length > 0 ? menus[0] : null;
		OnDeselect(null);
	}

	public void OnDeselect(BaseEventData eventData)
	{
		if (menu)
		{
			menu.SetSelected(null); 
		}
		if (text)
		{
			text.color = normalColor; 
		}
	}

	public void OnSelect(BaseEventData eventData)
	{
		if (menu)
		{
			menu.SetSelected(selectIndicatorPosition); 
		}
		if (text)
		{
			text.color = normalColor; 
		}
	}

	public void OnSubmit(BaseEventData eventData)
	{
	
	}

}
