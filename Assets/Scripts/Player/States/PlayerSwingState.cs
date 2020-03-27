using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSwingState : PlayerGrappleBaseState
{
	public float time = 0;
	public float minGrappleLength = 1f;
	public float swingSpeed = 10f;
	public float climbSpeed = 1f;
	public float mass = 10f;
	[Tooltip("Fall faster when grapple is not fully stretched")]
	public float gravityMultiplier = 3f;
	public bool doesResetDoubleJump = true;

	private float gravityMultiplierTolerance = 0.5f;
	private float maxVelocityMagnitude = 40;
	private Vector2 gravity = new Vector2(0, -1);
	private Vector2 grappleDirection;
	private float tensionForce;
	private Vector2 pendulumSideDirection;
	private Vector2 tangentDirection;
	private bool doesPlayerHaveParentAtStart = false;
	private bool isFacingRight;

	public override void Enter()
	{
		base.Enter();
		isFacingRight = player.isFacingRight;
		if (isFacingRight && player.spriteRenderer.flipX)
		{
			player.spriteRenderer.flipX = false;
		}
		else if (!isFacingRight && !player.spriteRenderer.flipX)
		{
			player.spriteRenderer.flipX = true;
		}
		player.playerAnimator.SetBool("Grappling", true);
		if (grappleLength < minGrappleLength)
		{
			grappleLength = minGrappleLength;
		}
		player.velocity.y = 0;
		if (doesResetDoubleJump)
		{
			player.doesDoubleJumpRemain = true;
		}
		player.ResetGrappleInputBuffer();

		if (player.lineRenderer != null && player.grappleDetection.currentGrapplePoint != null)
		{
			player.lineRenderer.SetPosition(0, player.transform.position);
			player.lineRenderer.SetPosition(1, player.grappleDetection.currentGrapplePoint.transform.position);
		}

		doesPlayerHaveParentAtStart = player.transform.parent != null;
		if (player.grappleDetection.currentGrapplePoint.GetComponentInParent<MovingPlatform>() != null && !doesPlayerHaveParentAtStart)
		{
			player.transform.parent = player.grappleDetection.currentGrapplePoint.GetComponentInParent<MovingPlatform>().transform;
			player.grappleDetection.currentGrapplePoint.GetComponentInParent<MovingPlatform>().isMoving = true;
		}
		
		player.lineRenderer.enabled = true;
	}

	public override void Update()
	{
		base.Update();

		if (player.grappleDetection.currentGrapplePoint != null)
		{
			float angle = Vector2.SignedAngle(player.transform.position - player.grappleDetection.currentGrapplePoint.transform.position, gravity);

			if (!isFacingRight)
			{
				float animationTime = Mathf.Clamp((angle + 90) / 180, 0, 1);
				player.playerAnimator.Play("Grapple", 0, animationTime);
			}
			else
			{
				float animationTime = Mathf.Clamp((-angle + 90) / 180, 0, 1);
				player.playerAnimator.Play("Grapple", 0, animationTime);
			}
		}

		if (player.isGrappleInputPressedBuffered && player.grappleDetection.nextGrapplePoint != null)
		{
			player.ResetGrappleInputBuffer();
			player.grappleDetection.ReleaseGrapplePoint();
			player.grappleDetection.SwitchCurrentNext();
			if (player.grappleDetection.grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Swing)
			{
				player.TransitionState(player.swingState);
			}
			else if (player.grappleDetection.grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Pull)
			{
				player.TransitionState(player.pullState);
			}
			else
			{
				player.TransitionState(player.whipState);
			}
			return;
		}
		else if (player.isGrappleInputPressedBuffered)
		{
			player.ResetGrappleInputBuffer();
			player.grappleDetection.ReleaseGrapplePoint();
			player.TransitionState(player.fallingState);
			return;
		}

		if (player.lineRenderer != null && player.grappleDetection.currentGrapplePoint != null)
		{
			player.lineRenderer.SetPosition(0, player.transform.position);
			player.lineRenderer.SetPosition(1, player.grappleDetection.currentGrapplePoint.transform.position);
		}
	}

	public override void FixedUpdate()
	{
		if (player.velocity.magnitude > maxVelocityMagnitude)
		{
			Vector2 maxVelocity = player.velocity.normalized * maxVelocityMagnitude;
			player.velocity = maxVelocity;
		}
		base.FixedUpdate();
		Vector2 playerPos = player.transform.position;
		Vector2 grapplePointPos = player.grappleDetection.currentGrapplePoint.transform.position;
		if (Vector2.Distance(grapplePointPos, playerPos) > minGrappleLength && Vector2.Distance(grapplePointPos, playerPos) < maxGrappleLength)
		{
			if (Mathf.Abs(player.verticalInputAxis) > 0)
			{
				float lastLength = grappleLength;
				grappleLength -= player.verticalInputAxis * climbSpeed * Time.deltaTime;
				float grappleDiference = lastLength - grappleLength;
				grappleLength = Mathf.Clamp(grappleLength, minGrappleLength, maxGrappleLength);
				grappleDirection = (grapplePointPos - playerPos).normalized;
				player.transform.position += grappleDiference * (Vector3)grappleDirection;
			}
		}
		if (Vector2.Angle(playerPos - grapplePointPos, gravity) > 150)
		{
			if (player.velocity.y > 0)
			{
				player.velocity.y = 0;
			}
		}
		if (Vector2.Distance(grapplePointPos, playerPos) + gravityMultiplierTolerance < grappleLength)
		{
			player.velocity += gravity.normalized * (gravity.magnitude * mass) * gravityMultiplier * Time.deltaTime;
		}
		else
		{
			player.velocity += gravity.normalized * (gravity.magnitude * mass) * Time.deltaTime;
		}
		if (player.CheckOverlaps(Vector2.up) && player.velocity.y > 0)
		{
			player.velocity = Vector2.zero;
		}
		if ((player.CheckOverlaps(Vector2.left) || player.CheckOverlaps(Vector2.right)) && Mathf.Abs(player.velocity.x) > 0)
		{
			player.velocity = Vector2.zero;
		}
		if (player.CheckOverlaps(Vector2.down))
		{
			if (Vector2.Distance(new Vector2(player.transform.position.x + player.horizontalInputAxis * player.walkingState.speed * Time.deltaTime, player.transform.position.y), player.grappleDetection.currentGrapplePoint.transform.position) < grappleLength)
			{
				player.MoveHorizontally(player.walkingState.speed, player.walkingState.acceleration, player.walkingState.deceleration);
			}
			else
			{
				player.velocity = Vector2.zero;
			}
		}
		else
		{
			float distanceAfterGravity = Vector2.Distance(grapplePointPos, playerPos + (player.velocity * Time.deltaTime));

			if (distanceAfterGravity > grappleLength || Mathf.Approximately(distanceAfterGravity, grappleLength))
			{
				grappleDirection = (grapplePointPos - playerPos).normalized;

				float angle = Vector2.Angle(playerPos - grapplePointPos, gravity);
				tensionForce = mass * gravity.magnitude * Mathf.Cos(Mathf.Deg2Rad * angle);
				tensionForce += ((mass * Mathf.Pow(player.velocity.magnitude, 2)) / grappleLength);

				player.velocity += grappleDirection * tensionForce * Time.deltaTime;

				if (angle < 70)
				{
					player.velocity += player.velocity.normalized * (player.velocity.x > 0 ? player.horizontalInputAxis : -player.horizontalInputAxis) * swingSpeed * Time.deltaTime;
				}
			}
		}
	}

	public override void Exit()
	{
		base.Exit();
		player.playerAnimator.SetBool("Grappling", false);
		if (!doesPlayerHaveParentAtStart)
		{
			player.transform.parent = null;
		}
		player.lineRenderer.enabled = false;
	}
}
