using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptSayDialog : MonoBehaviour
{
	public Canvas canvas { get; private set; }
	[HideInInspector] public bool isTemplate = true;

	RectTransform panel;

	private void Awake()
	{
		Debug.Log("Interrupt awake");
		canvas = GetComponent<Canvas>();
		panel = transform.GetChild(0) as RectTransform;
	}

	public void Appear()
	{
		canvas.sortingOrder = InterruptSay.GetNewTopSortingOrder();
		panel.Rotate(0, 0, Random.Range(-5f, 5f));
	}
}
