using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerStay : MonoBehaviour
{
	private void OnTriggerStay2D(Collider2D collision)
	{
		IKillable killable = collision.GetComponentInParent<IKillable>();
		if (killable != null)
		{
			killable.TakeDamage();
		}
	}
}
