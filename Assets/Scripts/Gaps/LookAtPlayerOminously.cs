using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerOminously : MonoBehaviour
{
	public Sprite[] sprites;
	public float maxAngle = 30f;
	public float rotateSpeed = 30f;

	public float currentAngle;
	private SpriteRenderer spriteRenderer;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 meToPlayer = PlayerController.get.transform.position - transform.position;
		float angleToPlayer = Mathf.Atan2(meToPlayer.y, meToPlayer.x) * Mathf.Rad2Deg + 90f;
		if (angleToPlayer > maxAngle)
		{
			currentAngle = Mathf.MoveTowards(currentAngle, 0, rotateSpeed * Time.deltaTime);
		}
		else
		{
			currentAngle = Mathf.MoveTowards(currentAngle, angleToPlayer, rotateSpeed * Time.deltaTime);
		}

		float normalizedAngle = Mathf.InverseLerp(-maxAngle, maxAngle, currentAngle);
		normalizedAngle = Mathf.Clamp01(normalizedAngle);

		int numSprites = sprites.Length;
		int spriteIndex = Mathf.RoundToInt(normalizedAngle * (numSprites - 1));
		spriteRenderer.sprite = sprites[spriteIndex];
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawLine(transform.position, transform.position + Util.Polar(5f, -90f - maxAngle));
		Gizmos.DrawLine(transform.position, transform.position + Util.Polar(5f, -90f + maxAngle));
	}
}
