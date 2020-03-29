using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeAndGoToScene : MonoBehaviour
{
	public string targetScene;
	public EasterEgg easterEgg;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerController.get.Freeze(true, true);
			StartCoroutine(WaitForFade());
		}
	}

	private IEnumerator WaitForFade()
	{
		yield return StartCoroutine(easterEgg.Fade(0, 1));
		SceneManager.LoadScene(targetScene);
	}
}
