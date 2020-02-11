using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
	public float upwardsSpeedGained = 20;
	public float minimumJumpDuration = 0.3f;
	public bool regainDoubleJump = true;

	public System.Action onBounce = delegate { };

	public virtual void Bounce(PlayerController player)
	{
		player.TransitionState(player.jumpingState);
		player.velocity.y = upwardsSpeedGained;
		if (regainDoubleJump)
		{
			player.doesDoubleJumpRemain = true;
		}
		player.jumpingState.minimumDurationOverride = minimumJumpDuration;
		onBounce.Invoke();
	}
}
