using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalWhipBehaviour : MonoBehaviour
{
	public float moveTimePeriod = 0.5f;
	public float moveSpeed = 5f;
	public float panicMoveSpeed = 10f;
	public float acceleration = 20f;
	public float deceleration = 20f;
	public float gravity = 10f;
	public float maxFallSpeed = 20f;

	public float maxAscendSpeed = 20f;
	public float maxHorizontalSpeed = 40f;

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
	private Vector2 velocity;
	private bool isPanicking;

	private State idleState;
	private State movingState;
	private State panicState;
	private StateMachine sm = new StateMachine();

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		rb2d = GetComponent<Rigidbody2D>();
		spriteRenderer.sprite = idleSprite;
		solidMask = LayerMask.GetMask("Solid");

		originalPosition = rb2d.position;

		idleState = new State(null, IdleStateEnter);
		movingState = new State(MovingStateUpdate, MovingStateEnter);
		panicState = new State(PanicStateUpdate, PanicStateEnter, PanicStateExit);

		sm.Transition(idleState);
	}

	private void OnEnable()
	{
		EventManager.StartListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnDisable()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
	}

	private void IdleStateEnter()
	{
		spriteRenderer.sprite = idleSprite;
	}

	private void MovingStateEnter()
	{
		moveTimer = moveTimePeriod;
		spriteRenderer.sprite = moveSprite;
	}

	private void MovingStateUpdate()
	{
		moveTimer -= Time.deltaTime;
		spriteRenderer.sprite = moveSprite;
		if (Mathf.Abs(velocity.x) < moveSpeed)
		{
			velocity.x = Mathf.MoveTowards(velocity.x, direction.x * moveSpeed, acceleration);
		}
	}

	private void PanicStateEnter()
	{
		spriteRenderer.sprite = moveSprite;
		spriteRenderer.color = Color.red;
		isPanicking = true;
	}

	private void PanicStateUpdate()
	{
		if (Mathf.Abs(velocity.x) < panicMoveSpeed)
		{
			velocity.x = Mathf.MoveTowards(velocity.x, direction.x * panicMoveSpeed, acceleration);
		}
	}
	
	private void PanicStateExit()
	{
		spriteRenderer.color = Color.white;
		isPanicking = false;
	}

	private void FixedUpdate()
	{
		velocity = rb2d.velocity;

		sm.Update();

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

		velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);

		if (velocity.x > 0)
		{
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
		else if (velocity.x < 0)
		{
			transform.rotation = Quaternion.Euler(0, 0, 180);
		}

		velocity.y -= gravity * Time.deltaTime;

		velocity.y = Mathf.Clamp(velocity.y, -maxFallSpeed, maxAscendSpeed);
		velocity.x = Mathf.Clamp(velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);

		rb2d.velocity = velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (isPanicking)
		{
			if (collision.GetContact(0).normal.x != 0)
			{
				rb2d.velocity = new Vector2(0, rb2d.velocity.y);
				StartCoroutine(StopPanickingBeforeNextFrame());
				if (collision.gameObject.TryGetComponent<BreakableWall>(out BreakableWall wall))
				{
					wall.Break();
				}
			}
		}
	}

	private IEnumerator StopPanickingBeforeNextFrame()
	{
		yield return new WaitForFixedUpdate();
		sm.Transition(idleState);
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
		sm.Transition(idleState);
	}

	public void Whip(GameObject player)
	{
		sm.Transition(movingState);
		if (player.transform.position.x > transform.position.x)
		{
			direction = Vector2.left;
		}
		else
		{
			direction = Vector2.right;
		}
	}

	public void Frighten(Vector2 directionToRun)
	{
		sm.Transition(panicState);
		direction = directionToRun;
	}
}
