using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
	public float playerAccelerationGliding = 60f;
	public float playerAccelerationFalling = 20f;
	public float animalWindSpeed = 50f;
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.GetComponentInParent<PlayerController>() != null)
		{
			PlayerController playerController = collision.GetComponentInParent<PlayerController>();
			playerController.windSpeed = new Vector2(Mathf.Sin(Mathf.Deg2Rad * -transform.rotation.eulerAngles.z), Mathf.Cos(Mathf.Deg2Rad * -transform.rotation.eulerAngles.z));
			if (playerController.currentState == playerController.glidingState)
			{
				playerController.windSpeed *= playerAccelerationGliding;
			}
			else if (playerController.currentState == playerController.fallingState)
			{
				playerController.windSpeed *= playerAccelerationFalling;
			}
			playerController.isInWind = true;
		}
		else if (collision.GetComponentInParent<AnimalWhipBehaviour>() != null)
		{
			AnimalWhipBehaviour animal = collision.GetComponentInParent<AnimalWhipBehaviour>();
			animal.windSpeed = new Vector2(Mathf.Sin(Mathf.Deg2Rad * -transform.rotation.eulerAngles.z), Mathf.Cos(Mathf.Deg2Rad * -transform.rotation.eulerAngles.z)) * animalWindSpeed;
			animal.isInWind = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponentInParent<PlayerController>() != null)
		{
			collision.GetComponentInParent<PlayerController>().isInWind = false;
		}
		else if (collision.GetComponentInParent<AnimalWhipBehaviour>() != null)
		{
			collision.GetComponentInParent<AnimalWhipBehaviour>().isInWind = false;
		}
	}
}
