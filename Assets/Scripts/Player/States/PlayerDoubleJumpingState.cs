
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
		if (player.TryGetComponent(out SpriteRenderer spriteRenderer))
		{
			spriteRenderer.flipY = true;
		}
	}

	public override void Exit()
	{
		base.Exit();
		if (player.TryGetComponent(out SpriteRenderer spriteRenderer))
		{
			spriteRenderer.flipY = false;
		}
	}
}
