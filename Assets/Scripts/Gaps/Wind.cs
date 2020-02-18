using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
	public float windSpeedFalling = 1f;
	public float windSpeedGliding = 2f;

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.GetComponent<PlayerController>() != null)
		{
			PlayerController playerController = collision.GetComponent<PlayerController>();
			if (playerController.currentState == playerController.glidingState)
			{
				playerController.glidingState.windSpeed = new Vector2(Mathf.Sin(Mathf.Deg2Rad * -transform.rotation.eulerAngles.z), Mathf.Cos(Mathf.Deg2Rad * -transform.rotation.eulerAngles.z)) * windSpeedGliding;
				playerController.glidingState.isInWind = true;
			}
			else if (playerController.currentState == playerController.fallingState)
			{
				playerController.fallingState.windSpeed = new Vector2(Mathf.Sin(Mathf.Deg2Rad * -transform.rotation.eulerAngles.z), Mathf.Cos(Mathf.Deg2Rad * -transform.rotation.eulerAngles.z)) * windSpeedFalling;
				playerController.fallingState.isInWind = true;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<PlayerController>() != null)
		{
			PlayerController playerController = collision.GetComponent<PlayerController>();
			playerController.fallingState.isInWind = false;
			playerController.glidingState.isInWind = false;
		}
	}
}
