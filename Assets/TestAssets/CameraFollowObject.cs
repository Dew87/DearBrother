using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
	public Rigidbody2D objectToFollow;
	public Vector3 followOffset = new Vector3(0, 0, -10);
	public float cameraFollowSpeed = 0.1f;
	public bool isStuckOnObject = false;
	public bool hasAcceleration = true;
	public Bounds bufferArea = new Bounds(Vector3.zero, new Vector3(2, 3));

	private void Update()
	{
		if (isStuckOnObject)
		{
			transform.position = objectToFollow.transform.position + followOffset;
		}
		else
		{
			Bounds worldSpaceBufferArea = bufferArea;
			worldSpaceBufferArea.center += transform.position;
			if (!worldSpaceBufferArea.Contains(objectToFollow.transform.position + followOffset) || objectToFollow.velocity == Vector2.zero)
			{
				float followSpeed;
				if (hasAcceleration)
				{
					followSpeed = Vector3.Distance(transform.position, objectToFollow.transform.position + followOffset) * cameraFollowSpeed * Time.deltaTime;
				}
				else
				{
					followSpeed = cameraFollowSpeed * Time.deltaTime;
				}
				transform.position = Vector3.MoveTowards(transform.position, (objectToFollow.transform.position + followOffset), followSpeed);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (objectToFollow)
		{
			Gizmos.DrawWireCube(objectToFollow.transform.position + followOffset + bufferArea.center, bufferArea.size);
		}
	}
}
