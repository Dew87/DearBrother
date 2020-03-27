using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerNormalJumpingState : PlayerBaseJumpingState
{
	[Tooltip("Delay before the player can double jump (seconds)")]
	[Space()]
	public float doubleJumpCooldown = 0.4f;

	private float doubleJumpTimer;

	public override void Enter()
	{
		base.Enter();

		player.soundManager.SamJumpSound();
		if (player.CheckForMovementSpeedModifier(out MovementSpeedModifier modifier))
		{
			if (modifier.jumpSpeed == 0)
			{
				player.TransitionState(player.previousState);
				return;
			}
			player.velocity.y = modifier.jumpSpeed;
		}

		doubleJumpTimer = doubleJumpCooldown;
	}

	public override void Update()
	{
		base.Update();

		if (doubleJumpTimer > 0)
		{
			doubleJumpTimer -= Time.deltaTime;
		}

		if (player.hasDoubleJump && player.doesDoubleJumpRemain && player.isJumpInputPressedBuffered && doubleJumpTimer <= 0)
		{
			player.TransitionState(player.doubleJumpingState);
		}
	}
}
