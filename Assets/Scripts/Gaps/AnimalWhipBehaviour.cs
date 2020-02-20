using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalWhipBehaviour : MonoBehaviour
{
	public float moveTimePeriod = 0.5f;
	public float moveSpeed = 5f;
	public float acceleration = 20f;
	public float deceleration = 20f;
	public float gravity = 10f;
	public float maxFallSpeed = 20f;

	public float maxAscendSpeed = 20f;
	public float maxHorizontalSpeed = 40f;

	public Sprite idleSprite;
	public Sprite moveSprite;
	public Collider2D solidCollider;

	[HideInInspector] public bool isInWind;
	[HideInInspector] public Vector2 windSpeed = Vector2.zero;

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
			if (Mathf.Abs(velocity.x) < moveSpeed)
			{
				velocity.x = Mathf.MoveTowards(velocity.x, direction.x * moveSpeed, acceleration); 
			}
		}
		else if (!isInWind)
		{
			spriteRenderer.sprite = idleSprite;
			velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
		}
		if (isInWind)
		{
			velocity += windSpeed * Time.deltaTime;
		}

		velocity.y -= gravity * Time.deltaTime;

		velocity.y = Mathf.Clamp(velocity.y, -maxFallSpeed, maxAscendSpeed);
		velocity.x = Mathf.Clamp(velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);

		rb2d.velocity = velocity;
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth player))
		{
			Bounds playerBounds = collision.collider.bounds;
			Bounds animalBounds = collision.otherCollider.bounds;
			PlayerController playerController = player.GetComponent<PlayerController>();
			Vector2 normal = collision.GetContact(0).normal;
			if (playerController.CheckOverlaps(Vector2.down) && rb2d.velocity.y < 0 && normal.y > 0)
			{
				player.TakeDamage();
			}
			else if (playerController.CheckOverlaps(Vector2.up) && rb2d.velocity.y > 0 && normal.y < 0)
			{
				player.TakeDamage();
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
