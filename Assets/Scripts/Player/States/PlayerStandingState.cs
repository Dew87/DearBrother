using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStandingState : PlayerState
{
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
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();

		player.RefillJumpGraceTimer();

		if (player.horizontalInputAxis != 0)
		{
			if (player.isCrouchInputHeld)
			{
				player.TransitionState(player.crawlingState);
			}
			else
			{
				player.TransitionState(player.walkingState);
			}
			return;
		}

		if (player.isCrouchInputHeld)
		{
			player.TransitionState(player.crouchingState);
			return;
		}

		if (player.isJumpInputPressedBuffered)
		{
			player.TransitionState(player.jumpingState);
			return;
		}

		if (!player.CheckOverlaps(Vector2.down))
		{
			player.TransitionState(player.fallingState);
			return;
		}

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
