using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrapplePointBehaviour : MonoBehaviour
{
	public enum GrappleType
	{
		Swing,
		Whip,
		Pull
	}

	public GrappleType grappleType;
	[Space]
	public UnityEvent whipEvent;
	[HideInInspector]public Rigidbody2D rb2d;

	private void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	public void UseGrapple()
	{
		if (grappleType == GrappleType.Whip)
		{
			whipEvent.Invoke();
		}
	}
}
