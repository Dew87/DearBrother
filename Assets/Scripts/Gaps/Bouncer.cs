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
	public ShakeConfig screenShake;

	public System.Action onBounce = delegate { };

	private Vector3 rotatedUp => Quaternion.Euler(0, 0, directionOffset) * transform.up;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.enabled)
		{
			if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
			{
				Bounce(player);
			}
			else if (collision.gameObject.TryGetComponent<AnimalWhipBehaviour>(out _))
			{
				Bounce(collision.rigidbody);
			}
		}
	}

	public void Bounce(PlayerController player)
	{
		if (!player.IsInCutscene && !player.isFrozen)
		{
			player.TransitionState(player.jumpingState);
			player.velocity = rotatedUp * speedGained;
			if (regainDoubleJump)
			{
				player.doesDoubleJumpRemain = true;
			}
			player.jumpingState.minimumDurationOverride = minimumJumpDuration;
			onBounce.Invoke();
			CameraShake.get.Shake(screenShake); 
		}
	}

	public void Bounce(Rigidbody2D rigidbody2D)
	{
		rigidbody2D.velocity = rotatedUp * speedGained;
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
