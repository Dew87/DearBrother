using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float inputThreshold = 0.1f;
	[Tooltip("If jump is pressed within this duration before touching the ground, the player will jump immediately after touching the ground")]
	public float jumpInputBuffer = 0.2f;
	[Tooltip("If jump is pressed within this duration after falling off a ledge, the player will jump in the air (coyote time)")]
	public float jumpGracePeriod = 0.2f;
	public float grappleInputBuffer = 0.2f;
	[Space()]
	public Collider2D normalCollider;
	public Collider2D crouchingCollider;
	public SpriteRenderer spriteRenderer;
	public GrappleDetection grappleDetection;
	public LineRenderer lineRenderer;

	[Header("States")]
	public PlayerStandingState standingState;
	public PlayerWalkingState walkingState;
	public PlayerCrouchingState crouchingState;
	public PlayerCrawlingState crawlingState;
	public PlayerNormalJumpingState jumpingState;
	public PlayerDoubleJumpingState doubleJumpingState;
	public PlayerFallingState fallingState;
	public PlayerGlidingState glidingState;
	public PlayerLandingLagState landingLagState;
	public PlayerSwingState swingState;
	public PlayerWhipState whipState;
	public PlayerPullState pullState;

	[Header("Debug")]
	[Tooltip("Is the double jump powerup unlocked?")]
	public bool hasDoubleJump = true;
	public bool doesDoubleJumpRemain;
	public Vector2 velocity;


	public float horizontalInputAxis { get; private set; }
	public float verticalInputAxis { get; private set; }
	public bool isFacingRight { get; private set; }
	public bool isJumpInputHeld { get; private set; }
	public bool isJumpInputPressedBuffered => jumpInputBufferTimer > 0;
	public bool isCrouchInputHeld { get; private set; }
	public bool isGrappleInputPressedBuffered => grappleInputBufferTimer > 0;
	public float jumpGraceTimer { get; private set; }
	public Rigidbody2D rb2d { get; private set; }
	public Collider2D currentCollider { get; private set; }

	public PlayerState previousState { get; private set; }
	public PlayerState currentState { get; private set; }

	private const float castDistance = 0.05f;

	private const float overlapDistance = 0.05f;
	private const float overlapSizeOffset = 0.02f;
	private int solidMask;
	private float jumpInputBufferTimer;
	private float grappleInputBufferTimer;
	private bool jumpInputIsTriggered;
	private bool grappleInputIsTriggered;

	private IEnumerable<PlayerState> IterateStates()
	{
		yield return standingState;
		yield return walkingState;
		yield return crouchingState;
		yield return crawlingState;
		yield return jumpingState;
		yield return doubleJumpingState;
		yield return fallingState;
		yield return glidingState;
		yield return landingLagState;
		yield return swingState;
		yield return whipState;
		yield return pullState;
	}

	private void Awake()
	{
		foreach (PlayerState state in IterateStates())
		{
			state.Awake();
		}
	}

	private void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		solidMask = LayerMask.GetMask("Solid");

		jumpInputIsTriggered = false;
		grappleInputIsTriggered = false;

		normalCollider.enabled = false;
		crouchingCollider.enabled = false;
		SetCollider(normalCollider);

		foreach (PlayerState state in IterateStates())
		{
			state.player = this;
		}

		TransitionState(standingState);

		foreach (PlayerState state in IterateStates())
		{
			currentState.Start();
		}
	}

	private void OnEnable()
	{
		EventManager.StartListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnDisable()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
	}

	private void Update()
	{
		ReadInput();

		currentState.Update();

		if (jumpGraceTimer > 0)
		{
			jumpGraceTimer -= Time.deltaTime;
		}
	}

	private void FixedUpdate()
	{
		currentState.FixedUpdate();

		rb2d.velocity = velocity;
	}

	public void MoveHorizontally(float speed, float acceleration, float deceleration)
	{
		if (Mathf.Abs(rb2d.velocity.x) > speed || horizontalInputAxis == 0)
		{
			velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
		}
		else
		{
			float effectiveAcceleration = acceleration;
			if (Mathf.Sign(horizontalInputAxis) == -Mathf.Sign(velocity.x))
			{
				effectiveAcceleration += deceleration;
			}
			velocity.x = Mathf.MoveTowards(velocity.x, speed * Mathf.Sign(horizontalInputAxis), effectiveAcceleration * Time.deltaTime);
		}
	}

	public void CheckForVolatilePlatforms()
	{
		RaycastHit2D[] hits = CheckOverlapsAll(Vector2.down);
		foreach (RaycastHit2D hit in hits)
		{
			if (hit.collider.TryGetComponent<VolatilePlatform>(out VolatilePlatform platform))
			{
				platform.Break();
			}
		}
	}

	public Collider2D CheckOverlaps(Vector2 direction)
	{
		Bounds bounds = currentCollider.bounds;

		RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, direction, castDistance, solidMask);

		return hit.collider;
	}

	public RaycastHit2D[] CheckOverlapsAll(Vector2 direction)
	{
		Bounds bounds = currentCollider.bounds;

		RaycastHit2D[] hits = Physics2D.BoxCastAll(bounds.center, bounds.size, 0f, direction, castDistance, solidMask);

		return hits;
	}

	public void ResetJumpInputBuffer()
	{
		jumpInputBufferTimer = 0;
	}

	public void RefillJumpGraceTimer()
	{
		jumpGraceTimer = jumpGracePeriod;
	}

	public void ResetJumpGraceTimer()
	{
		jumpGraceTimer = 0;
	}

	public void ResetGrappleInputBuffer()
	{
		grappleInputBufferTimer = 0;
	}

	public void SetCollider(Collider2D collider)
	{
		if (currentCollider)
		{
			currentCollider.enabled = false;
		}
		collider.enabled = true;
		currentCollider = collider;
	}

	public void TransitionState(PlayerState newState)
	{
		if (currentState != null)
		{
			currentState.isCurrentState = false;
			currentState.Exit();
		}
		previousState = currentState;
		currentState = newState;
		if (currentState != null)
		{
			currentState.Enter();
			currentState.isCurrentState = true;
		}
	}

	private void ReadInput()
	{
		if (jumpInputBufferTimer > 0)
		{
			jumpInputBufferTimer -= Time.deltaTime;
		}

		bool isGrappleInputHeld = Input.GetAxisRaw("Grapple") > inputThreshold;
		if (grappleInputBufferTimer > 0)
		{
			grappleInputBufferTimer -= Time.deltaTime;
		}
		else if (isGrappleInputHeld && !grappleInputIsTriggered)
		{
			grappleInputIsTriggered = true;
			grappleInputBufferTimer = grappleInputBuffer;
		}
		if (!isGrappleInputHeld)
		{
			grappleInputIsTriggered = false;
		}

		horizontalInputAxis = Input.GetAxisRaw("Horizontal");
		if (horizontalInputAxis != 0)
		{
			isFacingRight = horizontalInputAxis > 0 ? true : false;
		}

		verticalInputAxis = Input.GetAxisRaw("Vertical");

		isJumpInputHeld = Input.GetAxisRaw("Jump") > inputThreshold;
		if (isJumpInputHeld && !jumpInputIsTriggered)
		{
			jumpInputIsTriggered = true;
			jumpInputBufferTimer = jumpInputBuffer;
		}
		if (!isJumpInputHeld)
		{
			jumpInputIsTriggered = false;
		}

		isCrouchInputHeld = Input.GetAxisRaw("Vertical") < -inputThreshold;
	}

	private void OnPlayerDeath()
	{
		rb2d.position = CheckPoint.GetActiveCheckPointPosition();
		transform.position = rb2d.position; // Need to force-sync transform for camera snapping to work properly
		rb2d.velocity = Vector2.zero;
		velocity = Vector2.zero;
	}

	private void OnValidate()
	{
		foreach (PlayerState state in IterateStates())
		{
			state.OnValidate();
		}
	}
}
