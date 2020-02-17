using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolatilePlatform : MonoBehaviour
{
	public float timeBeforeFalling = 1f;

	private bool breaking = false;
	private SpriteRenderer spriteRenderer;
	private Vector3 originalPosition;
	private Color originalColor;
	private Vector3 originalScale;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		originalPosition = transform.position;
		originalColor = spriteRenderer.color;
		originalScale = transform.localScale;
		EventManager.StartListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnDestroy()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnPlayerDeath()
	{
		StopAllCoroutines();
		Debug.Log("Player died");
		transform.position = originalPosition;
		spriteRenderer.color = originalColor;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		GetComponent<Collider2D>().enabled = true;
		breaking = false;
		gameObject.SetActive(true);
	}

	public void Break()
	{
		if (!breaking)
		{
			breaking = true;
			StartCoroutine(AnimateBreak()); 
		}
	}

	private IEnumerator AnimateBreak()
	{
		const float flashPeriod = 0.15f;
		float timer = 0;
		while (timer < timeBeforeFalling)
		{
			timer += Time.deltaTime;
			spriteRenderer.color = Mathf.Repeat(timer, flashPeriod) > 0.5f * flashPeriod ? originalColor : Color.black;
			yield return null;
		}

		spriteRenderer.color = originalColor;

		float scale = 1;
		float duration = 2f;
		float speed = 0f;
		float gravity = 20f;
		float rotateSpeed = 50f;
		GetComponent<Collider2D>().enabled = false;
		while (scale > 0)
		{
			scale -= Time.deltaTime / duration;
			transform.localScale = originalScale * scale;

			speed += Time.deltaTime * gravity;
			transform.position += Vector3.down * speed * Time.deltaTime;

			transform.rotation *= Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime);

			yield return null;
		}

		gameObject.SetActive(false);
	}
}
