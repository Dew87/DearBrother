using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalWhipBehaviour : MonoBehaviour
{
	public float moveTimePeriod = 0.5f;
	public float moveSpeed = 5f;
	public float gravity = 10f;
	public float maxFallSpeed = 20f;

	public Sprite idleSprite;
	public Sprite moveSprite;
	public Collider2D solidCollider;

	private const float overlapDistance = 0.05f;

	private SpriteRenderer spriteRenderer;
	private Rigidbody2D rb2d;

	private float moveTimer;
	private Vector2 direction;
	private int solidMask;
	private Vector2 originalPosition;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		rb2d = GetComponent<Rigidbody2D>();
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

	private void FixedUpdate()
	{
		Vector2 velocity = rb2d.velocity;

		Bounds bounds = solidCollider.bounds;
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

		if (moveTimer > 0)
		{
			moveTimer -= Time.deltaTime;
			spriteRenderer.sprite = moveSprite;
			velocity.x = direction.x * moveSpeed;
		}
		else
		{
			spriteRenderer.sprite = idleSprite;
			velocity.x = 0;
		}
		velocity.y -= gravity * Time.deltaTime;
		if (velocity.y < -maxFallSpeed)
		{
			velocity.y = -maxFallSpeed;
		}

		rb2d.velocity = velocity;
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (rb2d.velocity.y < 0 && collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth player))
		{
			Bounds playerBounds = collision.collider.bounds;
			Bounds animalBounds = collision.otherCollider.bounds;
			PlayerController playerController = player.GetComponent<PlayerController>();
			if (playerController.CheckOverlaps(Vector2.down))
			{
				Vector2 normal = collision.GetContact(0).normal;
				if (normal.y > 0)
				{
					player.TakeDamage();
				}
			}
		}
	}

	private void OnPlayerDeath()
	{
		Respawn();
	}

	private void Respawn()
	{
		rb2d.position = originalPosition;
		rb2d.velocity = Vector2.zero;
		transform.rotation = Quaternion.identity;
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
