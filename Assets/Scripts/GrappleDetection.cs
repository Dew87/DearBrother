using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GrappleDetection : MonoBehaviour
{
	public float detectionRadius = 5f;
	public GameObject currentTargetCircle;
	public GameObject nextTargetCircle;

	public GameObject currentGrapplePoint;
	public GameObject nextGrapplePoint;
	public GrapplePointBehaviour grapplePointBehaviour;

	public PlayerController playerController;

	private GameObject nearestGrapplePoint;

	private bool isHolding = false;
	private float grappleReleaseCooldown = 0.05f;
	private float grappleReleaseTimer = 0;
	private LayerMask grappleMask;
	private LayerMask solidMask;

	private void OnDrawGizmos()
	{
		Handles.DrawWireArc(transform.position, Vector3.forward, playerController.isFacingRight ? Vector3.down : Vector3.up, 180f, detectionRadius);
		Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y - 5, transform.position.z), new Vector3(transform.position.x, transform.position.y + 5, transform.position.z));
	}

	private void Start()
	{
		grappleMask = LayerMask.GetMask("GrapplePoint");
		solidMask = LayerMask.GetMask("Solid");
	}

	private void Update()
	{
		if (grappleReleaseTimer > 0)
		{
			grappleReleaseTimer -= Time.deltaTime;
		}

		List<Collider2D> results = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, detectionRadius, grappleMask));
		List<Collider2D> colliders = new List<Collider2D>();
		int length = results.Count;
		for (int i = 0; i < length; i++)
		{
			if (results[i].gameObject != currentGrapplePoint)
			{
				if (playerController.isFacingRight)
				{
					if (results[i].transform.position.x >= transform.position.x && !Physics2D.Linecast(transform.position, results[i].transform.position, solidMask))
					{
						colliders.Add(results[i]);
					}
				}
				else
				{
					if (results[i].transform.position.x <= transform.position.x && !Physics2D.Linecast(transform.position, results[i].transform.position, solidMask))
					{
						colliders.Add(results[i]);
					}
				}
			}
		}

		int closestIndex = 0;
		for (int i = 0; i < colliders.Count; i++)
		{
			if (Vector2.Distance(transform.position, colliders[i].transform.position) < Vector2.Distance(transform.position, colliders[closestIndex].transform.position))
			{
				closestIndex = i;
			}
		}

		if (colliders.Count <= 0)
		{
			nextTargetCircle.SetActive(false);
		}
		else
		{
			nextTargetCircle.transform.position = colliders[closestIndex].transform.position;
			nextTargetCircle.SetActive(true);
		}

		if (isHolding)
		{
			if (colliders.Count > 0)
			{
				nextGrapplePoint = colliders[closestIndex].gameObject;
			}
			else
			{
				nextGrapplePoint = null;
			}
			currentTargetCircle.SetActive(true);
			currentTargetCircle.transform.position = currentGrapplePoint.transform.position;
		}
		else
		{
			currentTargetCircle.SetActive(false);
		}

		if (colliders.Count > 0 && grappleReleaseTimer <= 0)
		{
			if (colliders[closestIndex] != null)
			{
				if (playerController.isGrappleInputPressedBuffered)
				{
					currentGrapplePoint = colliders[closestIndex].gameObject;
					grapplePointBehaviour = currentGrapplePoint.GetComponent<GrapplePointBehaviour>();
					if (grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Swing || grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Pull)
					{
						isHolding = true;
					}
				}
			}
		}
	}

	public void ReleaseGrapplePoint()
	{
		grappleReleaseTimer = grappleReleaseCooldown;
		currentGrapplePoint = null;
		grapplePointBehaviour = null;
		isHolding = false;
	}

	public void SwitchCurrentNext()
	{
		currentGrapplePoint = nextGrapplePoint;
		nextGrapplePoint = null;
		grapplePointBehaviour = currentGrapplePoint.GetComponent<GrapplePointBehaviour>();
		if (grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Swing || grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Pull)
		{
			isHolding = true;
		}
	}
}
