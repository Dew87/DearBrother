using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
	public System.Action onHitPlayer = delegate { };

	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerHealth player = collision.GetComponentInParent<PlayerHealth>();
		if (player != null)
		{
			player.TakeDamage();
			//onHitPlayer.Invoke();
		}
	}
}
