using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EasterEgg : MonoBehaviour
{
	public RenderPipelineAsset rpa;
	public float fadeDuration = 0.5f;

	private SpriteRenderer spriteRenderer;

	// Start is called before the first frame update
	void Start()
	{
		GraphicsSettings.renderPipelineAsset = rpa;
		spriteRenderer = GetComponent<SpriteRenderer>();
		StartCoroutine(Fade(1, 0));
	}

	private void Update()
	{
		FMODUnity.RuntimeManager.MuteAllEvents(false);
	}

	private void OnDestroy()
	{
		GraphicsSettings.renderPipelineAsset = null;
	}

	public IEnumerator Fade(float from, float to)
	{
		Color color = spriteRenderer.color;

		float t = 0;
		while (t <= 1)
		{
			t += Time.deltaTime / fadeDuration;
			color.a = Mathf.Lerp(from, to, t);
			spriteRenderer.color = color;
			yield return null;
		}
	}
}
