using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWalkingState : PlayerState
{
	public float walkSpeed = 5;
	public float runSpeed = 10;
	public float acceleration = 20;
	public float deceleration = 20;

	public override void Enter()
	{
		base.Enter();

		player.velocity.y = 0;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		player.MoveHorizontally(player.isSprintInputHeld ? runSpeed : walkSpeed, acceleration, deceleration);
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();

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
	}
}
