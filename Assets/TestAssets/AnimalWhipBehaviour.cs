using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalWhipBehaviour : MonoBehaviour
{
	public float moveTimePeriod = 0.5f;
	public float moveSpeed = 5f;

	public Sprite idleSprite;
	public Sprite moveSprite;

	private const float overlapDistance = 0.05f;

	private SpriteRenderer spriteRenderer;
	private Rigidbody2D rb2d;
	private new Collider2D collider;

	private float moveTimer;
	private Vector2 direction;
	private int solidMask;
	private Vector2 originalPosition;

    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
		rb2d = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();
		spriteRenderer.sprite = idleSprite;
		solidMask = LayerMask.GetMask("Solid");

		originalPosition = rb2d.position;
    }

	private void OnEnable()
	{
		EventManager.StartListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnDisable()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnPlayerDeath()
	{
		Respawn();
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

	private void Respawn()
	{
		rb2d.position = originalPosition;
		rb2d.velocity = Vector2.zero;
		transform.rotation = Quaternion.identity;
	}

	private void FixedUpdate()
	{
		Bounds bounds = collider.bounds;
		float y = bounds.min.y;
		Vector2 position = new Vector2(bounds.center.x, y - overlapDistance * 0.5f);
		Vector2 size = new Vector2(bounds.size.x, overlapDistance);
		foreach (Collider2D collider in Physics2D.OverlapBoxAll(position, size, 0, solidMask))
		{
			if (collider.TryGetComponent<VolatilePlatform>(out VolatilePlatform platform))
			{
				platform.Break();
			}
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
