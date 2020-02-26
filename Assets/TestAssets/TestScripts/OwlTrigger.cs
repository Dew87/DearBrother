using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlTrigger : MonoBehaviour
{
	public Animator owlAnimator;
	public int triggerNumber = 0;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponentInParent<PlayerController>() != null)
		{
			owlAnimator.SetInteger("owl", triggerNumber);
		}
	}
}
