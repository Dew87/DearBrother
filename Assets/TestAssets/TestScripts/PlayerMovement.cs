using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	//Discontinued
	[Header("Ground movement")]
	public float walkSpeed = 5f;
	public float runSpeed = 10f;
	public float acceleration = 20f;
	public float deceleration = 20f;

	[Header("Air movement")]
	public float airAcceleration = 20f;
	public float airDeceleration = 10f;

	[Header("Jumping")]
	public float jumpSpeed = 20f;
	public float ascendGravity = 50f;
	public float descendGravity = 100f;
	public float jumpStopSpeed = 2f;
	public float jumpGracePeriod = 0.1f;
	public float jumpBufferPeriod = 0.1f;
	public float fallMaxSpeed = 20f;
	public float balloonFallMaxSpeed = 4f;
	public int maxJumpAmount = 1;

	[Header("WallJumping")]
	public float wallJumpGracePeriod = 0.2f;
	public float wallJumpSpeed = 5f;
	public float wallSlideSpeed = 1f;

	[Header("Landing")]
	public float landingLagPeriod = 0.2f;
	public float landingLagThreshold = 5f;

	[Header("Collision")]
	public const float castDistance = 0.05f;

	[Header("Debug")]
	public bool hasBalloonPower = false;
	public int availableJumps = 0;

	public float jumpGraceTimer = 0;
	public float jumpBufferTimer = 0;
	public float wallJumpGraceTimer = 0;
	public float landingLagTimer = 0;
	public float fallingTimer = 0;

	public Vector2 velocity;

	private Rigidbody2D rb2d;
	private new BoxCollider2D collider;
	private int solidMask;

	private void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		collider = GetComponent<BoxCollider2D>();
		solidMask = LayerMask.GetMask("Solid");
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && landingLagTimer <= 0)
		{
			jumpBufferTimer = jumpBufferPeriod;
			if (availableJumps < maxJumpAmount && availableJumps > 0)
			{
				velocity.y = jumpSpeed;
				availableJumps--;
			}
		}
		if (jumpGraceTimer > 0)
		{
			jumpGraceTimer -= Time.deltaTime;
		}
		if (wallJumpGraceTimer > 0)
		{
			wallJumpGraceTimer -= Time.deltaTime;
		}
		if (jumpBufferTimer > 0)
		{
			jumpBufferTimer -= Time.deltaTime;
		}
		if (landingLagTimer > 0)
		{
			landingLagTimer -= Time.deltaTime;
		}
	}

	private void FixedUpdate()
	{
		bool collidesDown = CheckOverlapss(Vector2.down);
		bool collidesUp = CheckOverlapss(Vector2.up);
		bool collidesLeft = CheckOverlapss(Vector2.left);
		bool collidesRight = CheckOverlapss(Vector2.right);

		float move = Input.GetAxisRaw("Horizontal");

		float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

		if (landingLagTimer <= 0)
		{
			if (Mathf.Abs(velocity.x) > speed || move == 0)
			{
				velocity.x = Mathf.MoveTowards(velocity.x, 0, (collidesDown ? deceleration : airDeceleration) * Time.deltaTime);
			}
			else
			{
				float effectiveAccel = collidesDown ? acceleration : airAcceleration;
				if (Mathf.Sign(move) == -Mathf.Sign(velocity.x))
				{
					effectiveAccel += collidesDown ? deceleration : airDeceleration;
				}
				velocity.x = Mathf.MoveTowards(velocity.x, speed * Mathf.Sign(move), effectiveAccel * Time.deltaTime);
			}
		}
		else
		{
			velocity.x = 0;
		}

		if (collidesDown)
		{
			if (jumpGraceTimer <= 0 && fallingTimer > landingLagThreshold)
			{
				landingLagTimer = landingLagPeriod;
			}
			fallingTimer = 0;
			jumpGraceTimer = jumpGracePeriod;
			availableJumps = maxJumpAmount;
		}
		else if (collidesLeft || collidesRight)
		{
			wallJumpGraceTimer = wallJumpGracePeriod;
			availableJumps = maxJumpAmount;
			fallingTimer = 0;
		}

		if (collidesUp && velocity.y > 0)
		{
			velocity.y = 0;
		}

		if ((collidesLeft && velocity.x < 0) || collidesRight && velocity.x > 0)
		{
			velocity.x = 0;
		}

		if (jumpGraceTimer > 0)
		{
			velocity.y = 0;
			if (jumpBufferTimer > 0)
			{
				velocity.y = jumpSpeed;
				jumpBufferTimer = 0;
				jumpGraceTimer = 0;
				availableJumps--;
			}
		}
		else
		{
			float gravity = velocity.y > 0 ? ascendGravity : descendGravity;
			velocity.y -= Time.deltaTime * gravity; //Velocity.y will not always be 0 when falling at descendGravity 0 since ascendGravity can make Velocity.y go past 0 to negative
			if (!Input.GetKey(KeyCode.Space) && velocity.y > jumpStopSpeed)
			{
				velocity.y = jumpStopSpeed;
			}
			velocity.y = Mathf.Max(velocity.y, hasBalloonPower ? -balloonFallMaxSpeed : -fallMaxSpeed);
			if (velocity.y < 0)
			{
				fallingTimer += -velocity.y * Time.deltaTime;
			}
		}

		if (velocity.y < wallSlideSpeed && (collidesLeft || collidesRight))
		{
			velocity.y = wallSlideSpeed;
		}

		if (wallJumpGraceTimer > 0)
		{
			if (jumpBufferTimer > 0)
			{
				velocity.y = jumpSpeed;
				if (collidesLeft)
				{
					velocity.x = wallJumpSpeed;
				}
				else if (collidesRight)
				{
					velocity.x = -wallJumpSpeed;
				}
				availableJumps = (availableJumps == maxJumpAmount) ? availableJumps - 1 : availableJumps;
			}
		}

		rb2d.velocity = velocity;
	}

	private bool CheckOverlapss(Vector2 direction)
	{
		Bounds bounds = collider.bounds;

		RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, direction, castDistance, solidMask);

		return hit.collider != null;
	}
}
