using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IKillable
{
	private PlayerController player;

	private IEnumerator FadeOut()
	{
		float alpha = 0;
		while (alpha < 1)
		{
			alpha += 1 / respawnTime * Time.unscaledDeltaTime;
			fadeOutSprite.color = new Color(fadeOutSprite.color.r, fadeOutSprite.color.g, fadeOutSprite.color.b, alpha);
			yield return null;
		}

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
		player = GetComponent<PlayerController>();
		fadeOutSprite.color = new Color(fadeOutSprite.color.r, fadeOutSprite.color.g, fadeOutSprite.color.b, 0);
	}

	public void TakeDamage()
	{
		if (!player.isFrozen)
		{
			Time.timeScale = 0;
			StartCoroutine(FadeOut());
		}
	}
}
