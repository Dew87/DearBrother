using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerJumpingState : PlayerState
{
	public float[] initialSpeed = new float[] { 20, 20 };
	public float[] gravity = new float[] { 20, 20 };
	[Space()]
	public float stopSpeed = 2;
	public float acceleration = 20;
	public float deceleration = 10;
	[Tooltip("Time (seconds) to wait before player can double jump/do their next air jump")]
	public float doubleJumpCooldown = 0.4f;

	private float doubleJumpCooldownTimer;

	private int jumpIndex;

	public override void Enter()
	{
		base.Enter();

		if (initialSpeed.Length == 0 || gravity.Length == 0)
		{
			Debug.LogError("Either the Initial speed or the Gravity array (or both) have a size of zero. (that's Bad)");
			return;
		}
		jumpIndex = player.maxAirJumps - player.airJumpsLeft;
		jumpIndex = Mathf.Clamp(jumpIndex, 0, Mathf.Min(initialSpeed.Length, gravity.Length) - 1);

		player.velocity.y = initialSpeed[jumpIndex];
		player.ResetJumpInputBuffer();
		doubleJumpCooldownTimer = doubleJumpCooldown;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		player.MoveHorizontally(player.isSprintInputHeld ? player.walkingState.runSpeed : player.walkingState.walkSpeed, acceleration, deceleration);

		player.velocity.y = Mathf.MoveTowards(player.velocity.y, 0, gravity[jumpIndex] * Time.deltaTime);

		if (!player.isJumpInputHeld && player.velocity.y > stopSpeed)
		{
			player.velocity.y = stopSpeed;
		}

		if (player.velocity.y > 0 && player.CheckBoxcast(Vector2.up))
		{
			player.velocity.y = 0;
		}

		if (player.airJumpsLeft > 0 && player.isJumpInputPressedBuffered && doubleJumpCooldownTimer <= 0)
		{
			player.airJumpsLeft--;
			Debug.LogError("air jump (from jump");
			player.TransitionState(player.jumpingState); // Yes, transition to self
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

		if (doubleJumpCooldownTimer > 0)
		{
			doubleJumpCooldownTimer -= Time.deltaTime;
		}
	}

	public override void OnValidate()
	{
		base.OnValidate();
	}
}
