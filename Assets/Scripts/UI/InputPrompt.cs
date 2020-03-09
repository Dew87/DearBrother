using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPrompt : MonoBehaviour
{
	public InputMethodSpriteChange imageObj;
	public Text textObj;
	public float fadeInDuration = 0.5f;
	public float fadeOutDuration = 0.5f;
	public float transitionMoveDistance = 10f;

	public static string currentPromptText => instance.textObj.text;

	private static InputPrompt instance;

	private CanvasGroup canvasGroup;
	private Vector3 basePos;

	// Start is called before the first frame update
	void Awake()
	{
		instance = this;
		canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0;
		basePos = transform.position;
	}

	public static void Show(string text, Sprite gamepadIcon, Sprite keyboardIcon)
	{
		if (!instance)
		{
			Debug.LogError("No InputPrompt in scene!");
			return;
		}

		instance.textObj.text = text;
		instance.imageObj.gamepadSprite = gamepadIcon;
		instance.imageObj.keyboardSprite = keyboardIcon;
		instance.imageObj.UpdateSprites();
		instance.StartCoroutine(instance.FadeIn());
	}

	public static void Hide()
	{
		if (!instance)
		{
			Debug.LogError("No InputPrompt in scene!");
			return;
		}

		instance.StartCoroutine(instance.FadeOut());
	}

	public void Hide(string promptTextToHide)
	{
		if (currentPromptText == promptTextToHide)
		{
			Hide();
		}
	}

	private IEnumerator FadeIn()
	{
		float t = 0;

		Vector3 startPos = transform.position + Vector3.down * transitionMoveDistance;

		while (t <= 1)
		{
			t += Time.deltaTime / fadeInDuration;
			canvasGroup.alpha = t;
			transform.position = Vector3.Lerp(startPos, basePos, t);
			yield return null;
		}
	}

	private IEnumerator FadeOut()
	{
		if (canvasGroup.alpha == 0)
		{
			yield break;
		}

		float t = 0;

		Vector3 startPos = transform.position;
		Vector3 targetPos = transform.position + Vector3.up * transitionMoveDistance;

		while (t <= 1)
		{
			t += Time.deltaTime / fadeOutDuration;
			canvasGroup.alpha = 1 - t;
			transform.position = Vector3.Lerp(startPos, targetPos, t);
			yield return null;
		}
	}
}
