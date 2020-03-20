using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDyingState : PlayerState
{
	public float startAnimationDelay = 0.2f;
	public float fadeOutDelay = 0.2f;
	public float fadeOutTime = 0.5f;
	public float fadeInDelay = 0.2f;
	public float fadeInTime = 0.5f;
	public float regainControlDelay = 0.5f;
	public ShakeConfig cameraShake;
	public SpriteRenderer fadeOutSprite;

	public override void Start()
	{
		base.Start();
		fadeOutSprite.color = new Color(fadeOutSprite.color.r, fadeOutSprite.color.g, fadeOutSprite.color.b, 0);
	}

	public override void Enter()
	{
		base.Enter();

		player.soundManager.StopSound();
		player.soundManager.PlayOneShot(player.soundManager.hurt);
		player.velocity = Vector3.zero;
		CameraShake.get.Shake(cameraShake);

		player.StartCoroutine(FadeOut());
		player.StartCoroutine(StartDeathAnimation());
	}

	public override void Update()
	{
		base.Update();
	}

	private IEnumerator StartDeathAnimation()
	{
		float t = 0;
		while (t < startAnimationDelay)
		{
			t += Time.deltaTime;
			yield return null;
		}
		player.playerAnimator.SetBool("Dead", true);
	}

	private IEnumerator FadeOut()
	{
		ForegroundObject[] foregroundObjects = GameObject.FindObjectsOfType<ForegroundObject>();

		yield return new WaitForSecondsRealtime(fadeOutDelay);

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
		player.playerAnimator.SetBool("Dead", false);
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

		player.TransitionState(player.standingState);
	}
}
