using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodTrigger : MonoBehaviour
{
	public PlayerMood mood;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerController.get.SetMood(mood);
		}
	}
}
