using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
	private IEnumerator FadeOut()
	{
		float alpha = 0;
		while (alpha < 1)
		{
			alpha += 1 / respawnTime * Time.unscaledDeltaTime;
			fadeOutSprite.color = new Color(fadeOutSprite.color.r, fadeOutSprite.color.g, fadeOutSprite.color.b, alpha);
			yield return null;
		}

		Debug.Log("PlayerDeath Triggered by " + this.ToString());
		EventManager.TriggerEvent("PlayerDeath");
		Time.timeScale = 1;

		alpha = 1;
		while (alpha > 0)
		{
			alpha -= 1 / respawnTime * Time.unscaledDeltaTime;
			fadeOutSprite.color = new Color(fadeOutSprite.color.r, fadeOutSprite.color.g, fadeOutSprite.color.b, alpha);
			yield return null;
		}

	}
	public float respawnTime = 0.5f;
	public SpriteRenderer fadeOutSprite;
	public void Start()
	{
		fadeOutSprite.color = new Color(fadeOutSprite.color.r, fadeOutSprite.color.g, fadeOutSprite.color.b, 0);
	}
	public void TakeDamage()
	{
		Time.timeScale = 0;
		StartCoroutine(FadeOut());
	}
}
