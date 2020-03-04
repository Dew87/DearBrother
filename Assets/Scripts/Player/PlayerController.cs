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
	public BoxCollider2D boxCollider2D;
	public Bounds standingColliderBounds = new Bounds(new Vector3(0, -0.0184f, 0), new Vector3(0.53f, 0.9632f, 0));
	public Bounds crouchingColliderBounds = new Bounds(new Vector3(0, -0.257f, 0), new Vector3(0.53f, 0.486f, 0));
	public SpriteRenderer spriteRenderer;
	public Animator playerAnimator;
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
	public bool hasFloat = true;
	public bool doesDoubleJumpRemain;
	public Vector2 velocity;

	public static PlayerController get { get; private set; }

	public float horizontalInputAxis { get; private set; }
	public float verticalInputAxis { get; private set; }
	public bool isFacingRight { get; private set; }
	public bool isJumpInputHeld { get; private set; }
	public bool isJumpInputPressedBuffered => jumpInputBufferTimer > 0;
	public bool isCrouchInputHeld { get; private set; }
	public bool isGrappleInputPressedBuffered => grappleInputBufferTimer > 0;
	public float jumpGraceTimer { get; private set; }
	public Rigidbody2D rb2d { get; private set; }
	public BoxCollider2D currentCollider { get; private set; }
	public int solidMask { get; private set; }
	public bool isFrozen { get; private set; }

	[HideInInspector] public bool isInWind = false;
	[HideInInspector] public Vector2 windSpeed = Vector2.zero;

	public PlayerState previousState { get; private set; }
	public PlayerState currentState { get; private set; }

	private const float overlapDistance = 0.01f;
	private const float overlapSizeOffset = 0.03f;

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
		get = this;

		foreach (PlayerState state in IterateStates())
		{
			state.Awake();
		}
	}

	private void Start()
	{

		rb2d = GetComponent<Rigidbody2D>();
		currentCollider = boxCollider2D;
		solidMask = LayerMask.GetMask("Solid", "SolidNoBlockGrapple");

		isFacingRight = false;
		jumpInputIsTriggered = false;
		grappleInputIsTriggered = false;
		SetCollider(standingColliderBounds);

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
		if (isFrozen) return;

		ReadInput();

		currentState.Update();

		if (jumpGraceTimer > 0)
		{
			jumpGraceTimer -= Time.deltaTime;
		}
	}

	private void FixedUpdate()
	{
		if (isFrozen) return;

		currentState.FixedUpdate();

		rb2d.velocity = velocity;

		// Dumb hack to prevent slightly sliding down slopes when landing on them
		// It's really only noticeable when not moving horizontally, so I just freeze the X position then
		if (velocity.x == 0)
		{
			rb2d.constraints |= RigidbodyConstraints2D.FreezePositionX;
		}
		else
		{
			rb2d.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
		}
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
		Collider2D[] allColliders = CheckOverlapsAll(Vector2.down);
		foreach (Collider2D collider in allColliders)
		{
			if (collider.TryGetComponent<VolatilePlatform>(out VolatilePlatform platform))
			{
				platform.Break();
			}
		}
	}

	public bool CheckForMovementSpeedModifier(out MovementSpeedModifier modifier)
	{
		Collider2D[] allColliders = CheckOverlapsAll(Vector2.down);
		foreach (Collider2D collider in allColliders)
		{
			if (collider.TryGetComponent<MovementSpeedModifier>(out modifier))
			{
				return true;
			}
		}
		modifier = null;
		return false;
	}

	public bool IsColliderOneWay(Collider2D collider)
	{
		if (collider.usedByEffector && collider.TryGetComponent<PlatformEffector2D>(out PlatformEffector2D platform))
		{
			if (platform.useOneWay)
			{
				return true;
			}
		}

		return false;
	}

	public Collider2D CheckOverlaps(Vector2 direction, float distance = overlapDistance)
	{
		Bounds bounds = currentCollider.bounds;

		if (direction.x == 0)
		{
			float y = direction.y < 0 ? bounds.min.y : bounds.max.y;
			Vector2 position = new Vector2(bounds.center.x, y + direction.y * overlapDistance * 0.5f);
			Vector2 size = new Vector2(bounds.size.x - overlapSizeOffset, distance);
			return Physics2D.OverlapBox(position, size, 0, solidMask);
		}
		else if (direction.y == 0)
		{
			float x = direction.x < 0 ? bounds.min.x : bounds.max.x;
			Vector2 position = new Vector2(x + direction.x * overlapDistance * 0.5f, bounds.center.y);
			Vector2 size = new Vector2(overlapDistance, bounds.size.y - distance);
			return Physics2D.OverlapBox(position, size, 0, solidMask);
		}
		else
		{
			Debug.LogError("Invalid CheckBoxcast direction " + direction);
			return null;
		}

	}

	public Collider2D[] CheckOverlapsAll(Vector2 direction)
	{
		return CheckOverlapsAll(direction, solidMask);
	}

	public Collider2D[] CheckOverlapsAll(Vector2 direction, int mask)
	{
		Bounds bounds = currentCollider.bounds;

		if (direction.x == 0)
		{
			float y = direction.y < 0 ? bounds.min.y : bounds.max.y;
			Vector2 position = new Vector2(bounds.center.x, y + direction.y * overlapDistance * 0.5f);
			Vector2 size = new Vector2(bounds.size.x - overlapSizeOffset, overlapDistance);
			return Physics2D.OverlapBoxAll(position, size, 0, mask);
		}
		else if (direction.y == 0)
		{
			float x = direction.x < 0 ? bounds.min.x : bounds.max.x;
			Vector2 position = new Vector2(x + direction.x * overlapDistance * 0.5f, bounds.center.y);
			Vector2 size = new Vector2(overlapDistance, bounds.size.y - overlapSizeOffset);
			return Physics2D.OverlapBoxAll(position, size, 0, mask);
		}
		else
		{
			Debug.LogError("Invalid CheckBoxcast direction " + direction);
			return null;
		}
	}

	public bool IsNormalColliderInWall()
	{
		Bounds bounds = standingColliderBounds;
		return Physics2D.OverlapBox(bounds.center, bounds.size, 0, solidMask);
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

	public void Freeze(bool freeze, bool resetVelocity = true)
	{
		if (resetVelocity)
		{
			velocity = Vector2.zero;
			rb2d.velocity = Vector2.zero; 
		}
		rb2d.simulated = !freeze;
		isFrozen = freeze;
	}

	public void SetCollider(Bounds colliderBounds)
	{
		currentCollider.size = colliderBounds.size;
		currentCollider.offset = colliderBounds.center;
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
		if (isFacingRight && spriteRenderer.flipX)
		{
			spriteRenderer.flipX = false;
		}
		else if (!isFacingRight && !spriteRenderer.flipX)
		{
			spriteRenderer.flipX = true;
		}

		verticalInputAxis = Input.GetAxisRaw("Vertical");
		isCrouchInputHeld = verticalInputAxis < -inputThreshold;

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
	}

	private void OnPlayerDeath()
	{
		rb2d.position = CheckPoint.GetActiveCheckPointPosition();
		transform.position = rb2d.position; // Need to force-sync transform for camera snapping to work properly
		rb2d.velocity = Vector2.zero;
		velocity = Vector2.zero;
		TransitionState(standingState);
		FindObjectOfType<PlayerCamera>().SnapToTarget();
	}

	private void OnValidate()
	{
		foreach (PlayerState state in IterateStates())
		{
			state.OnValidate();
		}
	}
}
