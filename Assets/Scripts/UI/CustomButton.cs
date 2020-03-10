using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, ISubmitHandler, ISelectHandler, IDeselectHandler
{
	public Text text;
	public GameObject selectIndicator;

	private void Awake()
	{
		OnDeselect(null);
	}

	public void OnDeselect(BaseEventData eventData)
	{
		selectIndicator.SetActive(false);
	}

	public void OnSelect(BaseEventData eventData)
	{
		selectIndicator.SetActive(true);
	}

	public void OnSubmit(BaseEventData eventData)
	{
		
	}
}
