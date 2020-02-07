using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerFallingState : PlayerState
{
	public float maxFallSpeed = 20;
	public float landingLagSpeedThreshold = 15;
	public float gravity = 100;
	public float acceleration = 20;
	public float deceleration = 10;

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();

		player.ResetJumpGraceTimer();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		player.MoveHorizontally(player.isSprintInputHeld ? player.walkingState.runSpeed : player.walkingState.walkSpeed, acceleration, deceleration);

		player.velocity.y = Mathf.MoveTowards(player.velocity.y, -maxFallSpeed, gravity * Time.deltaTime);

		if (player.CheckBoxcast(Vector2.down))
		{
			if (player.velocity.y > -landingLagSpeedThreshold)
			{
				if (player.velocity.x == 0)
				{
					player.TransitionState(player.standingState);
				}
				else
				{
					player.TransitionState(player.walkingState);
				}
			}
			else
			{
				player.TransitionState(player.landingLagState);
			}
			return;
		}

		if (player.jumpGraceTimer > 0 && player.isJumpInputPressedBuffered)
		{
			player.TransitionState(player.jumpingState);
		}
		else if (player.airJumpsLeft > 0 && player.isJumpInputPressedBuffered)
		{
			player.airJumpsLeft--;
			Debug.Log("air jump (from fall)");
			player.TransitionState(player.jumpingState);
		}
		else if (player.isJumpInputHeld && player.isGlideAvailable)
		{
			player.TransitionState(player.glidingState);
		}
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();
	}
}
