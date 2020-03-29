using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EasterEggGateway : MonoBehaviour
{
	public int triesRequired = 3;
	public string targetScene;

	private int tries = 0;

	private void OnEnable()
	{
		EventManager.StartListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnDisable()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnPlayerDeath()
	{
		if (tries >= triesRequired)
		{
			SceneManager.LoadScene(targetScene);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			tries++;
			PlayerController.get.GetComponent<PlayerHealth>().TakeDamage();
		}
	}
}
