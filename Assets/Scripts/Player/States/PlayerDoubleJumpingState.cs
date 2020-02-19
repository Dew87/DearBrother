using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDoubleJumpingState : PlayerBaseJumpingState
{
	public override void Enter()
	{
		base.Enter();
		player.doesDoubleJumpRemain = false;
		player.spriteRenderer.flipY = true;
	}

	public override void Exit()
	{
		base.Exit();
		player.spriteRenderer.flipY = false;
	}
}
