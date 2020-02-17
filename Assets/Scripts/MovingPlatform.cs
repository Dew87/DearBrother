using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	public Collider2D platformCollider;
	public List<Transform> pathPoints;
	public bool isMovingOneDirection = false;
	public float moveSpeed = 5f;
	public float tolerance = 1f;

	private float castDistance = 0.05f;
	private int currentPositionInPath;
	private bool isMovingForward;
	private void OnDrawGizmos()
	{
		for (int i = 0; i < pathPoints.Count; i++)
		{
			Gizmos.DrawWireSphere(pathPoints[i].position, 0.05f);
			if (isMovingOneDirection)
			{
				if (i < pathPoints.Count - 1)
				{
					Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
				}
			}
			else
			{
				if (i < pathPoints.Count - 1)
				{
					Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
				}
				else
				{
					Gizmos.DrawLine(pathPoints[i].position, pathPoints[0].position);
				}
			}
		}
	}
	private void Start()
    {
		int closestIndex = 0;
		for (int i = 0; i < pathPoints.Count; i++)
		{
			if (Vector2.Distance(transform.position, pathPoints[i].position) < Vector2.Distance(transform.position, pathPoints[closestIndex].position))
			{
				closestIndex = i;
			}
		}
		currentPositionInPath = closestIndex;
    }

    private void FixedUpdate()
    {
		transform.position = Vector2.MoveTowards(transform.position, pathPoints[currentPositionInPath].position, Time.deltaTime * moveSpeed);
		if (Vector2.Distance(transform.position, pathPoints[currentPositionInPath].position) < tolerance)
		{
			if (!isMovingOneDirection)
			{
				currentPositionInPath++;
				if (currentPositionInPath == pathPoints.Count)
				{
					currentPositionInPath = 0;
				}
			}
			else
			{
				currentPositionInPath += isMovingForward ? 1 : -1;
				if (currentPositionInPath == pathPoints.Count)
				{
					isMovingForward = false;
					currentPositionInPath = pathPoints.Count - 1;
				}
				else if (currentPositionInPath == -1)
				{
					isMovingForward = true;
					currentPositionInPath = 1;
				}
			}
		}
    }
	private void OnCollisionEnter2D(Collision2D collision)
	{
		collision.gameObject.transform.parent = transform;
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		collision.gameObject.transform.parent = null;
	}
}
