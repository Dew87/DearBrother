using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCrouchingState : PlayerState
{
	public override void Enter()
	{
		base.Enter();
		player.velocity.x = 0;
		player.playerAnimator.SetBool("Crouching", true);
		player.playerAnimator.SetBool("Moving", false);
		player.SetCollider(player.crouchingColliderBounds);
	}

	public override void Exit()
	{
		base.Exit();
		player.playerAnimator.SetBool("Crouching", false);
		player.SetCollider(player.standingColliderBounds);
	}

	public override void Update()
	{
		base.Update();

		player.ResetJumpGraceTimer();

		bool canStand = !player.IsNormalColliderInWall();

		if (!player.isCrouchInputHeld && canStand)
		{
			player.TransitionState(player.standingState);
			return;
		}

		if (player.horizontalInputAxis != 0)
		{
			player.TransitionState(player.crawlingState);
			return;
		}

		if (player.isJumpInputPressedBuffered && canStand)
		{
			player.TransitionState(player.jumpingState);
			return;
		}

		if (!player.CheckOverlaps(Vector2.down))
		{
			player.TransitionState(player.fallingState);
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
}
