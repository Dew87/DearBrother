using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

	[Header("Debug")]

	public bool balloonPower = false;

	private const float raycastDistance = 0.05f;

	[SerializeField] private Vector2 velocity;
	private float jumpGraceTimer = 0;
	private float jumpBufferTimer = 0;

	private Rigidbody2D rb2d;
	private new BoxCollider2D collider;
	private int solidMask;

	void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		collider = GetComponent<BoxCollider2D>();
		solidMask = LayerMask.GetMask("Solid");
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		RaycastHit2D[] results = new RaycastHit2D[1];

		bool collidesDown = CheckRaycasts(Vector2.down);
		bool collidesUp = CheckRaycasts(Vector2.up);
		bool collidesLeft = CheckRaycasts(Vector2.left);
		bool collidesRight = CheckRaycasts(Vector2.right);


		float move = Input.GetAxisRaw("Horizontal");

		float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

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

		if (collidesDown)
		{
			jumpGraceTimer = jumpGracePeriod;
		}

		if (collidesUp && velocity.y > 0)
		{
			velocity.y = 0;
		}

		if ((collidesLeft && velocity.x < 0) || collidesRight && velocity.x > 0)
		{
			velocity.x = 0;
		}

		if (jumpBufferTimer > 0)
		{
			jumpBufferTimer -= Time.deltaTime;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			jumpBufferTimer = jumpBufferPeriod;
		}

		if (jumpGraceTimer > 0)
		{
			jumpGraceTimer -= Time.deltaTime;
			velocity.y = 0;
			if (jumpBufferTimer > 0)
			{
				velocity.y = jumpSpeed;
				jumpBufferTimer = 0;
				jumpGraceTimer = 0;
			}
		}
		else
		{
			float gravity = velocity.y > 0 ? ascendGravity : descendGravity;
			velocity.y -= Time.deltaTime * gravity;
			if (!Input.GetKey(KeyCode.Space) && velocity.y > jumpStopSpeed)
			{
				velocity.y = jumpStopSpeed;
			}

			velocity.y = Mathf.Max(velocity.y, balloonPower ? -balloonFallMaxSpeed : -fallMaxSpeed);
		}

		//MoveAndSlide(velocity * Time.deltaTime);
		rb2d.velocity = velocity;

	}

	private bool CheckRaycasts(Vector2 direction)
	{
		Bounds bounds = collider.bounds;

		if (direction.x == 0)
		{
			float y = direction.y < 0 ? bounds.min.y : bounds.max.y;
			RaycastHit2D hitL = Physics2D.Raycast(new Vector2(bounds.min.x, y), direction, raycastDistance, solidMask);
			RaycastHit2D hitM = Physics2D.Raycast(new Vector2(bounds.center.x, y), direction, raycastDistance, solidMask);
			RaycastHit2D hitR = Physics2D.Raycast(new Vector2(bounds.max.x, y), direction, raycastDistance, solidMask);
			return hitL.collider || hitM.collider || hitR.collider;
		}
		else if (direction.y == 0)
		{
			float x = direction.x < 0 ? bounds.min.x : bounds.max.x;
			RaycastHit2D hitU = Physics2D.Raycast(new Vector2(x, bounds.min.y), direction, raycastDistance, solidMask);
			RaycastHit2D hitM = Physics2D.Raycast(new Vector2(x, bounds.center.y), direction, raycastDistance, solidMask);
			RaycastHit2D hitD = Physics2D.Raycast(new Vector2(x, bounds.max.y), direction, raycastDistance, solidMask);
			return hitU.collider || hitM.collider || hitD.collider;
		}

		Debug.LogError("Invalid direction " + direction, this);
		return false;
	}
}
