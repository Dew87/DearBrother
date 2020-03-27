using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeAndGoToScene : MonoBehaviour
{
	public string targetScene;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			SceneManager.LoadSceneAsync(targetScene);
			PlayerController.get.Freeze(true, true);
		}
	}
}
