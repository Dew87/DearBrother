using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IKillable
{
	private PlayerController player;

	public void Start()
	{
		player = GetComponent<PlayerController>();
	}

	public void TakeDamage()
	{
		if (!player.isFrozen && player.currentState != player.dyingState)
		{
			player.TransitionState(player.dyingState);
		}
	}
}
