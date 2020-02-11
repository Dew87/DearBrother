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

	private GameObject nearestGrapplePoint;

	private bool isFacingRight = true;
	private bool isHolding = false;

	private LayerMask grappleMask;
	private LayerMask solidMask;
	private void OnDrawGizmos()
	{
		Handles.DrawWireArc(transform.position, Vector3.forward, isFacingRight ? Vector3.down : Vector3.up, 180f, detectionRadius);
		Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y - 5, transform.position.z), new Vector3(transform.position.x, transform.position.y + 5, transform.position.z));
	}
	private void Start()
	{
		grappleMask = LayerMask.GetMask("GrapplePoint");
		solidMask = LayerMask.GetMask("Solid");
	}
	private void Update()
	{
		float move = Input.GetAxis("Horizontal");
		if (move != 0)
		{
			isFacingRight = move > 0 ? true : false;
		}
		List<Collider2D> results = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, detectionRadius, grappleMask));
		List<Collider2D> colliders = new List<Collider2D>();
		int length = results.Count;
		for (int i = 0; i < length; i++)
		{
			if (results[i].gameObject != currentGrapplePoint)
			{
				if (isFacingRight)
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
		if (colliders.Count > 0)
		{
			if (colliders[closestIndex] != null)
			{
				if (Input.GetKeyDown(KeyCode.E))
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
