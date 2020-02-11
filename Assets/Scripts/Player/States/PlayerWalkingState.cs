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

		player.MoveHorizontally(speed, acceleration, deceleration);
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

		if (!player.CheckBoxcast(Vector2.down))
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
