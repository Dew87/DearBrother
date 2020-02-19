using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
	[UnityEngine.Serialization.FormerlySerializedAs("upwardsSpeedGained")]
	public float speedGained = 20;
	public float minimumJumpDuration = 0.3f;
	public bool regainDoubleJump = true;
	[Tooltip("Angle offset from rotation to use as bouncing direction (degrees, counter-clockwise). Leave at 0 to use local up direction.")]
	public float directionOffset = 0;

	public System.Action onBounce = delegate { };

	private Vector3 rotatedUp => Quaternion.Euler(0, 0, directionOffset) * transform.up;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.enabled && collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
		{
			Bounce(player);
		}
	}

	public virtual void Bounce(PlayerController player)
	{
		player.TransitionState(player.jumpingState);
		player.velocity = rotatedUp * speedGained;
		if (regainDoubleJump)
		{
			player.doesDoubleJumpRemain = true;
		}
		player.jumpingState.minimumDurationOverride = minimumJumpDuration;
		onBounce.Invoke();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		GizmosExt.DrawArrow(transform.position, rotatedUp);
	}

	private void OnValidate()
	{
		if (TryGetComponent<PlatformEffector2D>(out PlatformEffector2D effector))
		{
			effector.rotationalOffset = directionOffset;
		}
		else
		{
			Debug.LogWarning("Bouncer should probably have a PlatformEffector");
		}
	}
}
