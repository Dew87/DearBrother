using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GrappleDetection : MonoBehaviour
{
	public float detectionRadius = 5f;
	[Range(0f, 180f)]
	public float detectionAngle = 90f;
	[Range(-45f, 45f)]
	public float detectionOffset = 0f;
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
		bool isFacingRight = playerController.isFacingRight;
		Handles.DrawWireArc(transform.position, Vector3.forward, new Vector3((isFacingRight ? 1 : -1) * Mathf.Cos(((0.5f * detectionAngle) + detectionOffset) * Mathf.Deg2Rad) * detectionRadius, Mathf.Sin(((0.5f * detectionAngle) + detectionOffset) * Mathf.Deg2Rad) * detectionRadius, 0), (isFacingRight ? -1 : 1) * detectionAngle, detectionRadius);
		Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + (isFacingRight ? 1 : -1) * Mathf.Cos(((0.5f * detectionAngle) + detectionOffset) * Mathf.Deg2Rad) * detectionRadius, transform.position.y + Mathf.Sin(((0.5f * detectionAngle) + detectionOffset) * Mathf.Deg2Rad) * detectionRadius, transform.position.z));
		Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + (isFacingRight ? 1 : -1) * Mathf.Cos(((0.5f * detectionAngle) - detectionOffset) * Mathf.Deg2Rad) * detectionRadius, transform.position.y - Mathf.Sin(((0.5f * detectionAngle) - detectionOffset) * Mathf.Deg2Rad) * detectionRadius, transform.position.z));
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
		bool isFacingRight = playerController.isFacingRight;
		int length = results.Count;
		for (int i = 0; i < length; i++)
		{
			if (results[i].gameObject != currentGrapplePoint)
			{
				RaycastHit2D hit = Physics2D.Linecast(transform.position, results[i].transform.position, solidMask);
				float angle = (isFacingRight ? 1 : -1) * Vector2.SignedAngle(isFacingRight ? Vector2.right : Vector2.left, (results[i].transform.position - transform.position).normalized);
				if (angle <= (0.5f * detectionAngle) +  detectionOffset && angle >= (0.5f * -detectionAngle) + detectionOffset)
				{
					colliders.Add(results[i]);
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
