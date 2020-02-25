using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
	public float playerAccelerationGliding = 60f;
	public float playerAccelerationFalling = 20f;
	public float animalWindSpeed = 50f;
	public ParticleSystem particles;
	public Vector2 offset;
	public Vector2 size;

	public bool isBlocked = false;
	private ParticleSystem.EmissionModule emission;
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position + (Vector3)offset, size);
	}
	private void Start()
	{
		emission = particles.emission;
	}
	private void Update()
	{
		isBlocked = CheckIfBlocked(Physics2D.OverlapBoxAll((Vector2)transform.position + offset, size, 0));
		if (isBlocked)
		{
			emission.enabled = false;
		}
		else
		{
			emission.enabled = true;
		}
	}
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
			playerController.isInWind = !isBlocked;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponentInParent<PlayerController>() != null)
		{
			collision.GetComponentInParent<PlayerController>().isInWind = false;
		}
	}
	private bool CheckIfBlocked(Collider2D[] collisions)
	{
		for (int i = 0; i < collisions.Length; i++)
		{
			if (collisions[i].GetComponentInParent<AnimalWhipBehaviour>() != null)
			{
				return true;
			}
		}
		return false;
	}
}
