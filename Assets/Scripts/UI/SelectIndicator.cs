using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectIndicator : MonoBehaviour
{
	public float wavePeriodX = 1;
	public float wavePeriodY = 1;
	public float waveAmplitudeX = 10;
	public float waveAmplitudeY = 10;
	public float timeOffsetY = 0;

	private Transform image;

	private void Start()
	{
		image = transform.GetChild(0);
	}

	private void Update()
	{
		float x = Mathf.Sin(Time.unscaledTime * 2 * Mathf.PI / wavePeriodX) * waveAmplitudeX;
		float y = Mathf.Sin((Time.unscaledTime + timeOffsetY) * 2 * Mathf.PI / wavePeriodY) * waveAmplitudeY;
		image.localPosition = new Vector2(x, y);
	}
}
