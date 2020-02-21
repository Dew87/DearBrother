using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptSayDialog : MonoBehaviour
{
	public Canvas canvas { get; private set; }
	[HideInInspector] public bool isTemplate = true;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
	}
}
