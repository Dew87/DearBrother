using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryController : MonoBehaviour
{
	public Graphic overlay;
	public Graphic image;
	[Space()]
	public float overlayFadeInDuration = 1f;
	public float imageFadeInDuration = 1f;
	public float imageFadeInDelay = 0.5f;
	[Space()]
	public float overlayFadeOutDuration = 1f;
	public float imageFadeOutDuration = 1f;
	public float overlayFadeOutDelay = 0.5f;

	public static MemoryController get { get; private set; }
	public static bool isOpen => get != null && get.gameObject.activeInHierarchy;

	private Animator imageAnimator;
	private Color overlayColor;
	private Color imageColor;
	private List<Collectible> collectedList = new List<Collectible>();

	private void Awake()
	{
		get = this;
		imageAnimator = image.GetComponent<Animator>();
	}

	private void Start()
	{
		overlayColor = overlay.color;
		imageColor = image.color;
		gameObject.SetActive(false);
	}

	public List<Collectible> GetCollectedMemories()
	{
		return collectedList;
	}

	public void CollectMemory(Collectible memory)
	{
		collectedList.Add(memory);
	}

	public IEnumerator Open(string animation)
	{
		gameObject.SetActive(true);
		imageAnimator.Play(animation);
		imageAnimator.speed = 0;

		float t = 0;
		float duration = Mathf.Max(overlayFadeInDuration, imageFadeInDelay + imageFadeInDuration);

		while (t <= duration)
		{
			overlay.color = Color.Lerp(Color.clear, overlayColor, t / overlayFadeInDuration);
			image.color = Color.Lerp(Color.clear, imageColor, (t - imageFadeInDelay) / imageFadeInDuration);
			t += Time.unscaledDeltaTime;
			if (t - imageFadeInDelay >= imageFadeInDuration)
			{
				imageAnimator.speed = 1; 
			}
			yield return null;
		}
	}

	public IEnumerator Close()
	{
		float t = 0;
		float duration = Mathf.Max(overlayFadeOutDuration, overlayFadeOutDelay + imageFadeOutDuration);

		Color overlayColor = overlay.color;
		Color imageColor = image.color;

		while (t <= duration)
		{
			overlay.color = Color.Lerp(overlayColor, Color.clear, (t - overlayFadeOutDelay) / overlayFadeOutDuration);
			image.color = Color.Lerp(imageColor, Color.clear, t / imageFadeOutDuration);
			t += Time.unscaledDeltaTime;
			yield return null;
		}

		gameObject.SetActive(false);
	}
}
