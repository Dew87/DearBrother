using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalWhipBehaviour : MonoBehaviour
{
	public float moveTimePeriod = 0.5f;
	public float moveSpeed = 5f;

	public Sprite idleSprite;
	public Sprite moveSprite;

	private SpriteRenderer spriteRenderer;
	private Rigidbody2D rb2d;

	private float moveTimer;
	private Vector2 direction;

	private LayerMask playerMask;
    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
		rb2d = GetComponent<Rigidbody2D>();
		spriteRenderer.sprite = idleSprite;
    }
    void Update()
    {
        if (moveTimer > 0)
		{
			moveTimer -= Time.deltaTime;
			spriteRenderer.sprite = moveSprite;
			rb2d.velocity = direction * moveSpeed;
		}
		else
		{
			spriteRenderer.sprite = idleSprite;
			rb2d.velocity = Vector2.zero;
		}
    }
	public void Whip(GameObject player)
	{
		moveTimer = moveTimePeriod;
		if (player.transform.position.x > transform.position.x)
		{
			if (Mathf.Approximately(transform.rotation.eulerAngles.y, 0))
			{
				transform.Rotate(Vector3.up, 180);
			}
			direction = Vector2.left;
		}
		else
		{
			if (Mathf.Approximately(transform.rotation.eulerAngles.y, 180))
			{
				transform.Rotate(Vector3.up, 180);
			}
			direction = Vector2.right;
		}
	}
}
