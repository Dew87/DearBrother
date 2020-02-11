using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSwingState : PlayerGrappleBaseState
{
	public float swingSpeed = 10f;
	public float mass = 10f;
	[Tooltip("Fall faster when grapple is not fully stretched")]
	public float gravityMultiplier = 3f;

	public bool doesResetDoubleJump = true;

	private float gravityMultiplierTolerance = 0.5f;

	private Vector2 gravity = new Vector2(0, -1);
	private Vector2 grappleDirection;
	private float tensionForce;
	private Vector2 pendulumSideDirection;
	private Vector2 tangentDirection;
	public override void Enter()
	{
		base.Enter();
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
		player.lineRenderer.enabled = true;
	}

	public override void Update()
	{
		base.Update();
		if (player.isGrappleInputPressedBuffered && player.grappleDetection.nextGrapplePoint != null)
		{
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
		base.FixedUpdate();

		if (player.CheckBoxcast(Vector2.up) && player.velocity.y > 0)
		{
			player.velocity = Vector2.zero;
		}
		if ((player.CheckBoxcast(Vector2.left) || player.CheckBoxcast(Vector2.right)) && Mathf.Abs(player.velocity.x) > 0)
		{
			player.velocity = Vector2.zero;
		}
		if (player.CheckBoxcast(Vector2.down))
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
			Vector2 playerPos = player.transform.position;
			Vector2 grapplePointPos = player.grappleDetection.currentGrapplePoint.transform.position;

			if (Vector2.Distance(grapplePointPos, playerPos) + gravityMultiplierTolerance < grappleLength)
			{
				player.velocity += gravity.normalized * (gravity.magnitude * mass) * gravityMultiplier * Time.deltaTime;
			}
			else
			{
				player.velocity += gravity.normalized * (gravity.magnitude * mass) * Time.deltaTime;
			}


			float distanceAfterGravity = Vector2.Distance(grapplePointPos, playerPos + (player.velocity * Time.deltaTime));

			if (distanceAfterGravity > grappleLength || Mathf.Approximately(distanceAfterGravity, grappleLength))
			{
				grappleDirection = (grapplePointPos - playerPos).normalized;

				float angle = Vector2.Angle(playerPos - grapplePointPos, gravity);

				tensionForce = mass * gravity.magnitude * Mathf.Cos(Mathf.Deg2Rad * angle);
				tensionForce += ((mass * Mathf.Pow(player.velocity.magnitude, 2)) / grappleLength);

				player.velocity += grappleDirection * tensionForce * Time.deltaTime;

				player.velocity += player.velocity.normalized * (player.velocity.x > 0 ? player.horizontalInputAxis : -player.horizontalInputAxis) * swingSpeed * Time.deltaTime;
			}
		}
	}
	public override void Exit()
	{
		base.Exit();
		player.lineRenderer.enabled = false;
	}
}
