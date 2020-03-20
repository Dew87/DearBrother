using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTarManSpeed : MonoBehaviour
{
	public float speed;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		TarManController tarMan = collision.GetComponent<TarManController>();
		if (tarMan != null)
		{
			tarMan.walkingState.speed = speed;
		}
	}
}
