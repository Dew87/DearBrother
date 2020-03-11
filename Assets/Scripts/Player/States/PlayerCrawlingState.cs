using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCrawlingState : PlayerState
{
	public float speed = 2;
	public float acceleration = 20;
	public float deceleration = 20;

	public override void Enter()
	{
		base.Enter();
		player.SetCollider(player.crouchingColliderBounds);
		player.spriteRenderer.transform.localScale = new Vector3(1, 0.5f);
	}

	public override void Exit()
	{
		base.Exit();
		player.SetCollider(player.standingColliderBounds);
		player.spriteRenderer.transform.localScale = Vector3.one;
	}

	public override void Update()
	{
		base.Update();

		player.ResetJumpGraceTimer();

		bool canStand = !player.IsNormalColliderInWall();

		if (!player.isCrouchInputHeld && canStand)
		{
			player.TransitionState(player.walkingState);
			return;
		}

		if (player.velocity.x == 0)
		{
			player.TransitionState(player.crouchingState);
			return;
		}

		if (player.isJumpInputPressedBuffered && canStand)
		{
			player.TransitionState(player.jumpingState);
			return;
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

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		float maxSpeed = speed;
		if (player.CheckForMovementSpeedModifier(out MovementSpeedModifier modifier))
		{
			maxSpeed = modifier.crawlSpeed;
		}

		player.MoveHorizontally(maxSpeed, acceleration, deceleration);

		if (!player.CheckOverlaps(Vector2.down))
		{
			player.TransitionState(player.fallingState);
			return;
		}
	}
}