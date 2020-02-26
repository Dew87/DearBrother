using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWalkingState : PlayerState
{
	public float speed = 4.3f;
	public float acceleration = 40f;
	public float deceleration = 30f;

	public override void Enter()
	{
		base.Enter();

		player.velocity.y = 0;
		player.doesDoubleJumpRemain = true;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		player.velocity.y = 0;

		float maxSpeed = speed;
		if (player.CheckForMovementSpeedModifier(out MovementSpeedModifier modifier))
		{
			maxSpeed = modifier.walkSpeed;
		}

		player.MoveHorizontally(maxSpeed, acceleration, deceleration);
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();

		player.RefillJumpGraceTimer();

		if (player.horizontalInputAxis == 0 && player.rb2d.velocity.x == 0)
		{
			if (player.isCrouchInputHeld)
			{
				player.TransitionState(player.crouchingState);
			}
			else
			{
				player.TransitionState(player.standingState);
			}
			return;
		}

		if (player.isCrouchInputHeld)
		{
			player.TransitionState(player.crawlingState);
			return;
		}

		if (player.isJumpInputPressedBuffered)
		{
			player.TransitionState(player.jumpingState);
			return;
		}

		if (!player.CheckOverlaps(Vector2.down))
		{
			Bounds bounds = player.currentCollider.bounds;
			float behindX = player.velocity.x > 0 ? bounds.min.x : bounds.max.x;
			RaycastHit2D slopeBehind = Physics2D.Raycast(new Vector2(behindX, bounds.min.y), Vector2.down, 0.2f, player.solidMask);
			if (slopeBehind)
			{
				player.rb2d.position += Vector2.down * slopeBehind.distance;
			}
			else
			{
				player.TransitionState(player.fallingState);
				return;
			}
		}

		player.CheckForVolatilePlatforms();

		if (player.isGrappleInputPressedBuffered && player.grappleDetection.currentGrapplePoint != null)
		{
			if (player.grappleDetection.grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Swing)
			{
				player.TransitionState(player.swingState);
			}
			else if (player.grappleDetection.grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Pull)
			{
				player.TransitionState(player.pullState);
			}
			else if (player.grappleDetection.grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Whip)
			{
				player.TransitionState(player.whipState);
			}
			return;
		}
	}
}
