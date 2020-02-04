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
    public int maxJumpAmount = 1;

	public bool balloonPower = false;

	private const float raycastDistance = 0.05f;
	private const float collisionTestOffset = 0.02f;

	[SerializeField] private Vector2 velocity;
	private float jumpGraceTimer = 0;
	private float jumpBufferTimer = 0;

    private int availableJumps = 0;

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

		bool grounded = CheckRaycasts(Vector2.down);
		if (grounded)
		{
			jumpGraceTimer = jumpGracePeriod;
            availableJumps = maxJumpAmount;
		}
		bool headCollision = CheckRaycasts(Vector2.up);

		float move = Input.GetAxisRaw("Horizontal");

		float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

		if (Mathf.Abs(velocity.x) > speed || move == 0)
		{
			velocity.x = Mathf.MoveTowards(velocity.x, 0, (grounded ? deceleration : airDeceleration) * Time.deltaTime);
		}
		else
		{
			float effectiveAccel = grounded ? acceleration : airAcceleration;
			if (Mathf.Sign(move) == -Mathf.Sign(velocity.x))
			{
				effectiveAccel += grounded ? deceleration : airDeceleration;
			}
			velocity.x = Mathf.MoveTowards(velocity.x, speed * Mathf.Sign(move), effectiveAccel * Time.deltaTime);
		}

		if (headCollision && velocity.y > 0)
		{
			velocity.y = 0;
		}

		if (jumpBufferTimer > 0)
		{
			jumpBufferTimer -= Time.deltaTime;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
            if (availableJumps < maxJumpAmount && availableJumps > 0)
            {
                velocity.y = jumpSpeed;
                availableJumps--;
            }
            else
            {
                jumpBufferTimer = jumpBufferPeriod;
            }
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
                availableJumps--;
            }
		}
		else
		{
			float gravity = velocity.y > 0 ? ascendGravity : descendGravity; 
			velocity.y -= Time.deltaTime * gravity; //Velocity.y will not always be 0 when falling at descendGravity is 0 since ascendGravity can make Velocity.y go past 0 to negative
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

	bool DetectGround()
	{
		Bounds bbox = collider.bounds;
		RaycastHit2D hitCenter = Physics2D.Raycast(new Vector2(bbox.center.x, bbox.min.y - 0.01f), Vector2.down, raycastDistance);
		Debug.DrawRay(new Vector2(bbox.center.x, bbox.min.y), Vector2.down * raycastDistance, Color.yellow);
		return hitCenter.collider != null;
	}

	private Vector2 MoveAndCollide(Vector2 movement)
	{
		Vector2 direction = movement.normalized;
		float length = movement.magnitude;
		if (length > 0)
		{
			RaycastHit2D[] hits = new RaycastHit2D[1];
			int numHits = rb2d.Cast(direction, hits, length);
			if (numHits > 0)
			{
				RaycastHit2D hit = hits[0];
				Vector2 move = direction * (hit.distance - Physics2D.defaultContactOffset);
				rb2d.position += move;
				return move;
			}
			else
			{
				rb2d.position += movement;
				return movement;
			}
		}

		return Vector2.zero;
	}

	private Vector2 MoveAndSlide(Vector2 movement)
	{
		Vector2 moved = MoveAndCollide(movement);
		Vector2 remainingMovement = movement - moved;

		return moved;
	}
}
