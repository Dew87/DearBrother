using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
	public ParticleSystem particles;
	public Vector2 blockOffset;
	public Vector2 blockSize;

	public bool isBlocked = false;
	public Vector2 windSpeed;
	private ParticleSystem.EmissionModule emission;
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position + (Vector3)blockOffset, blockSize);
	}
	private void Start()
	{
		emission = particles.emission;
		windSpeed = new Vector2(Mathf.Sin(Mathf.Deg2Rad * -transform.rotation.eulerAngles.z), Mathf.Cos(Mathf.Deg2Rad * -transform.rotation.eulerAngles.z));
	}
	private void Update()
	{
		isBlocked = CheckIfBlocked(Physics2D.OverlapBoxAll((Vector2)transform.position + blockOffset, blockSize, 0));
		if (isBlocked)
		{
			emission.enabled = false;
		}
		else
		{
			emission.enabled = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerController playerController = collision.GetComponentInParent<PlayerController>();
		if (playerController != null)
		{
			playerController.windSpeed += windSpeed;
			playerController.isInWind = !isBlocked;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		PlayerController playerController = collision.GetComponentInParent<PlayerController>();
		if (playerController != null)
		{
			playerController.windSpeed -= windSpeed;
			playerController.isInWind = false;
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
