using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerEnter : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		IKillable killable = collision.GetComponentInParent<IKillable>();
		if (killable != null)
		{
			killable.TakeDamage();
		}
	}
}
