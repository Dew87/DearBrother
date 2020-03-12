﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IKillable
{
	public float fadeOutTime = 0.5f;
	public float fadeInDelay = 0.2f;
	public float fadeInTime = 0.5f;
	public SpriteRenderer fadeOutSprite;

	private PlayerController player;

	public void Start()
	{
		player = GetComponent<PlayerController>();
		fadeOutSprite.color = new Color(fadeOutSprite.color.r, fadeOutSprite.color.g, fadeOutSprite.color.b, 0);
	}

	public void TakeDamage()
	{
		if (!player.isFrozen)
		{
			player.soundManager.StopSound();
			player.soundManager.PlayOneShot(player.soundManager.hurt);
			Time.timeScale = 0;
			PlayerCamera.get.useUnscaledTime = true;
			StartCoroutine(FadeOut());
		}
	}

	private IEnumerator FadeOut()
	{
		player.Freeze(true);

		ForegroundObject[] foregroundObjects = FindObjectsOfType<ForegroundObject>();

		float alpha = 0;
		while (alpha < 1)
		{
			alpha += 1 / fadeInTime * Time.unscaledDeltaTime;
			fadeOutSprite.color = new Color(fadeOutSprite.color.r, fadeOutSprite.color.g, fadeOutSprite.color.b, alpha);
			foreach (var obj in foregroundObjects)
			{
				obj.SetFadeAlpha(1 - alpha);
			}
			yield return null;
		}


		yield return new WaitForSecondsRealtime(fadeInDelay);

		PlayerCamera.get.useUnscaledTime = false;
		Vector3 cameraRelativePosition = PlayerCamera.get.followOffsetTransform.position - PlayerController.get.transform.position;
		EventManager.TriggerEvent("PlayerDeath");
		PlayerCamera.get.transform.position = PlayerController.get.transform.position + cameraRelativePosition - PlayerCamera.get.followOffsetTransform.localPosition;

		yield return null;

		Time.timeScale = 1;
		player.Freeze(false);

		alpha = 1;
		while (alpha > 0)
		{
			alpha -= 1 / fadeOutTime * Time.unscaledDeltaTime;
			fadeOutSprite.color = new Color(fadeOutSprite.color.r, fadeOutSprite.color.g, fadeOutSprite.color.b, alpha);
			foreach (var obj in foregroundObjects)
			{
				obj.SetFadeAlpha(1 - alpha);
			}
			yield return null;
		}
	}
}
