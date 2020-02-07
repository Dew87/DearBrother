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

		if (!player.CheckBoxcast(Vector2.down))
		{
			player.TransitionState(player.fallingState);
			return;
		}
	}
}
