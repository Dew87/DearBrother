using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerJumpingState : PlayerState
{
	public float initialSpeed = 20;
	public float gravity = 50;
	public float stopSpeed = 2;
	public float acceleration = 20;
	public float deceleration = 10;

	public override void Enter()
	{
		base.Enter();

		player.velocity.y = initialSpeed;
		player.ResetJumpInputBuffer();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		player.MoveHorizontally(player.isSprintInputHeld ? player.walkingState.runSpeed : player.walkingState.walkSpeed, acceleration, deceleration);

		player.velocity.y = Mathf.MoveTowards(player.velocity.y, 0, gravity * Time.deltaTime);

		if (!player.isJumpInputHeld && player.velocity.y > stopSpeed)
		{
			player.velocity.y = stopSpeed;
		}

		if (player.velocity.y > 0 && player.CheckBoxcast(Vector2.up))
		{
			player.velocity.y = 0;
		}
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();

		if (player.velocity.y <= 0)
		{
			player.TransitionState(player.fallingState);
		}
	}
}
