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
	public float jumpGracePeriod = 0.2f;

	private float jumpGraceTimer;

	public override void Enter()
	{
		base.Enter();

		if (player.previousState != player.jumpingState)
		{
			jumpGraceTimer = jumpGracePeriod; 
		}
	}

	public override void Exit()
	{
		base.Exit();

		jumpGraceTimer = 0;
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

		if (jumpGraceTimer > 0 && player.isJumpInputPressedBuffered)
		{
			player.TransitionState(player.jumpingState);
		}
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();

		if (player.isJumpInputHeld && player.isGlideAvailable)
		{
			player.TransitionState(player.glidingState);
		}

		jumpGraceTimer -= Time.deltaTime;
	}
}
