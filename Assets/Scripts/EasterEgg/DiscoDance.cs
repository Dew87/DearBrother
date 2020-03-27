using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoDance : MonoBehaviour
{
	public float scaleX = 0.5f;
	public float scaleY = 0.5f;
	public float period = 0.25f;
	public float flipInterval = 0.7f;

	float timer = 0;

	private void Update()
	{
		float sx = 1 + Mathf.Sin(Time.time * 2 * Mathf.PI / period) * scaleX;
		float sy = 1 + Mathf.Sin((Time.time * 2 * Mathf.PI + period) / period) * scaleY;
		transform.localScale = new Vector2(sx, sy);

		timer += Time.deltaTime;
		if (timer > flipInterval)
		{
			timer = 0;
			transform.Rotate(0, 180, 0);
		}
	}
}
