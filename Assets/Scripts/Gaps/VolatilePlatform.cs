using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolatilePlatform : MonoBehaviour
{
	[Tooltip("If player's fall speed is larger than this when they land, the platform breaks")]
	public float breakSpeed = 2f;

	public void Break()
	{
		StartCoroutine(AnimateBreak());
	}

	private IEnumerator AnimateBreak()
	{
		float scale = 1;
		float duration = 2f;
		float speed = 0f;
		float gravity = 20f;
		float rotateSpeed = 50f;
		GetComponent<Collider2D>().enabled = false;
		Vector3 baseScale = transform.localScale;
		while (scale > 0)
		{
			scale -= Time.deltaTime / duration;
			transform.localScale = baseScale * scale;


			speed += Time.deltaTime * gravity;
			transform.position += Vector3.down * speed * Time.deltaTime;

			transform.rotation *= Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime);

			yield return null;
		}

		gameObject.SetActive(false);
	}
}
