using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingPlatform : MonoBehaviour
{
	public Collider2D platformCollider;
	public List<Transform> pathPoints;
	public bool isMovingOneDirection = false;
	public float moveSpeed = 5f;
	public float waitTime = 1f;
	public float tolerance = 0.1f;
	public float decelerationDistance = 1f;

	[HideInInspector] public bool isMoving = false;

	private List<Collider2D> childrenColliders = new List<Collider2D>();

	private float overlapDistance = 0.075f;

	private int currentPositionInPath;
	private bool isMovingForward;
	private float waitTimer = 0f;
	private float currentMoveSpeed;

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
		if (waitTimer <= 0 && isMoving)
		{
			transform.position = Vector2.MoveTowards(transform.position, pathPoints[currentPositionInPath].position, Time.deltaTime * currentMoveSpeed);
		}
    }
	private void Update()
	{
		Bounds bounds = platformCollider.bounds;
		float y = bounds.max.y;
		Vector2 position = new Vector2(bounds.center.x, y + overlapDistance * 0.5f);
		Vector2 size = new Vector2(bounds.size.x, overlapDistance);
		List<Collider2D> colliders = new List<Collider2D>();
		colliders.AddRange(Physics2D.OverlapBoxAll(position, size, 0));
		for (int i = 0; i < colliders.Count; i++)
		{
			if (!childrenColliders.Contains(colliders[i]))
			{
				if (colliders[i].gameObject.GetComponentInParent<Rigidbody2D>() != null && colliders[i].gameObject.GetComponentInParent<TilemapCollider2D>() == null && colliders[i].gameObject.transform.parent.gameObject != gameObject)
				{
					colliders[i].gameObject.transform.parent.parent = gameObject.transform;
					childrenColliders.Add(colliders[i]);
					if (colliders[i].gameObject.GetComponentInParent<PlayerController>() != null)
					{
						isMoving = true;
					}
				}
			}
		}
		for (int i = 0; i < childrenColliders.Count; i++)
		{
			if (!colliders.Contains(childrenColliders[i]))
			{
				childrenColliders[i].gameObject.transform.parent.parent = null;
				childrenColliders.Remove(childrenColliders[i]);
			}
		}

		float currentDistanceToNextPoint = Vector2.Distance(transform.position, pathPoints[currentPositionInPath].position);
		currentMoveSpeed = moveSpeed;
		if (isMovingOneDirection)
		{
			if (waitTimer > 0)
			{
				waitTimer -= Time.deltaTime;
			}
			if (currentPositionInPath == pathPoints.Count - 1 || currentPositionInPath == 0)
			{
				if (currentDistanceToNextPoint < decelerationDistance)
				{
					currentMoveSpeed = (currentDistanceToNextPoint / decelerationDistance) * moveSpeed;
				}
			}
		}
		if (currentDistanceToNextPoint < tolerance)
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
					waitTimer = waitTime;
					isMovingForward = false;
					currentPositionInPath = pathPoints.Count - 1;
				}
				else if (currentPositionInPath == -1)
				{
					waitTimer = waitTime;
					isMovingForward = true;
					currentPositionInPath = 1;
				}
			}
		}
	}
}
