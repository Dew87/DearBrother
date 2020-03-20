using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
	public Graphic title;
	public Graphic pressAnyKey;
	[Space()]
	public float vignetteIntensity = 0.73f;
	public float vignetteTransitionDuration = 1f;
	public float zoom = 1.5f;
	public float zoomRestoreDuration = 1f;
	[Space()]
	public float titleFadeDuration = 1f;
	public float pressAnyKeyFadeDuration = 0.5f;

	private bool hasStartedGame = false;
	private float shakeMultiplier = 1;

	private PostProcessVolume postProcess;
	private Vignette vignette;
	private float normalVignette;

	private void Start()
	{
		postProcess = FindObjectOfType<PostProcessVolume>();
		vignette = postProcess.profile.GetSetting<Vignette>();
		normalVignette = vignette.intensity;
		vignette.intensity.value = vignetteIntensity;
		PlayerCamera.get.SetZoom(zoom, 0);
	}

	private void Update()
	{
		if (!hasStartedGame)
		{
			if (Input.anyKeyDown)
			{
				StartGame();
			}
			else
			{
				foreach (var key in InputManager.joystickButtons)
				{
					if (Input.GetKeyDown(key))
					{
						StartGame();
					}
				}
			}
		}
	}

	private void StartGame()
	{
		hasStartedGame = true;
		PlayerController.get.introState.RiseUp();
		StartCoroutine(TweenVignette());
		StartCoroutine(TweenTitle());
		StartCoroutine(TweenPressAnyKey());
		PlayerCamera.get.SetZoom(1, zoomRestoreDuration);
	}

	private IEnumerator TweenVignette()
	{
		float t = 0;
		while (t <= 1)
		{
			t += Time.deltaTime / vignetteTransitionDuration;
			vignette.intensity.value = Mathf.Lerp(vignetteIntensity, normalVignette, t);
			yield return null;
		}
	}

	private IEnumerator TweenTitle()
	{
		float t = 0;
		while (t <= 1)
		{
			t += Time.deltaTime / titleFadeDuration;
			title.color = new Color(1, 1, 1, 1 - t);
			yield return null;
		}
	}

	private IEnumerator TweenPressAnyKey()
	{
		pressAnyKey.GetComponent<Flash>().enabled = false;
		pressAnyKey.color = Color.white;

		float t = 0;
		while (t <= 1)
		{
			t += Time.deltaTime / pressAnyKeyFadeDuration;
			pressAnyKey.color = new Color(1, 1, 1, 1 - t);
			yield return null;
		}
	}
}
