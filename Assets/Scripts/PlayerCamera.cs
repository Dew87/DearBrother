using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	public bool snapToTargetOnStart = true;
	public Rigidbody2D objectToFollow;
	public Vector3 followOffset;
	public float cameraSpeedWhenStill = 2f;

	public float lookDownSpeed = 20f;
	public float lookDownDelay = 0.2f;
	public float lookDownDistance = 6f;
	public bool onlyLookWhenStill = true;

	private Collider2D objectToFollowCollider;
	private LayerMask solidMask;
	private float lookDownTimer;
	[System.Serializable]
	public struct Extents
	{
		public float x, up, down;

		public Extents(float x, float up, float down)
		{
			this.x = x;
			this.up = up;
			this.down = down;
		}

		public Extents(float x, float y)
		{
			this.x = x;
			this.up = y;
			this.down = y;
		}

		public Vector2 size => new Vector2(x * 2, up + down);
		public Vector3 localCenter => new Vector3(0, (up - down) * .5f, 0);
	}

	public Extents bufferArea = new Extents(1, 2, 0.5f);

	// Start is called before the first frame update
	void Start()
	{
		if (snapToTargetOnStart)
		{
			SnapToTarget();
		}
		Collider2D[] colliders = new Collider2D[1];
		objectToFollow.GetAttachedColliders(colliders);
		if (colliders.Length > 0)
		{
			if (colliders[0] != null)
			{
				objectToFollowCollider = colliders[0];
			}
		}
		solidMask = LayerMask.GetMask("Solid");
	}

	private void OnEnable()
	{
		EventManager.StartListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnDisable()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 currentPosition = transform.position;
		Vector3 newCameraPosition = currentPosition;

		Vector3 followPosition = objectToFollow.transform.position + followOffset;
		Vector2 objectVelocity = objectToFollow.velocity;

		if (Mathf.Abs(objectToFollow.velocity.x) >= cameraSpeedWhenStill)
		{
			if (followPosition.x > currentPosition.x + bufferArea.x)
			{
				newCameraPosition.x = followPosition.x - bufferArea.x;
			}
			else if (followPosition.x < currentPosition.x - bufferArea.x)
			{
				newCameraPosition.x = followPosition.x + bufferArea.x;
			}
		}
		else
		{
			newCameraPosition.x = Mathf.MoveTowards(newCameraPosition.x, followPosition.x, cameraSpeedWhenStill * Time.deltaTime);
		}

		if (Mathf.Abs(objectToFollow.velocity.y) > cameraSpeedWhenStill)
		{
			if (followPosition.y > currentPosition.y + bufferArea.up)
			{
				newCameraPosition.y = followPosition.y - bufferArea.up;
			}
			else if (followPosition.y < currentPosition.y - bufferArea.down)
			{
				newCameraPosition.y = followPosition.y + bufferArea.down;
			}
		}
		else
		{
			newCameraPosition.y = Mathf.MoveTowards(newCameraPosition.y, followPosition.y, cameraSpeedWhenStill * Time.deltaTime);
		}

		Bounds bounds = objectToFollowCollider.bounds;
		bool grounded = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, 0.05f, solidMask);

		bool isAbleToMove = onlyLookWhenStill ? Mathf.Approximately(objectToFollow.velocity.x, 0) : true;
		if (lookDownTimer >= lookDownDelay)
		{
			newCameraPosition.y = Mathf.MoveTowards(newCameraPosition.y, followPosition.y - lookDownDistance, lookDownSpeed * Time.deltaTime);
		}

		bool doLookDownTimer = Input.GetAxis("Vertical") < 0 && grounded && isAbleToMove;
		lookDownTimer = doLookDownTimer ? lookDownTimer + Time.deltaTime : 0;
		if (lookDownTimer <= 0 && newCameraPosition.y < followPosition.y - bufferArea.down && grounded)
		{
			newCameraPosition.y = Mathf.MoveTowards(newCameraPosition.y, followPosition.y, lookDownSpeed * Time.deltaTime);
		}

		transform.position = newCameraPosition;
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Vector3 position = transform.position;
			position.z = 0;
			Gizmos.DrawWireCube(position + bufferArea.localCenter - followOffset, bufferArea.size);
		}
		else
		{
			Gizmos.DrawWireCube(objectToFollow.transform.position + bufferArea.localCenter, bufferArea.size);
		}
	}

	private void SnapToTarget()
	{
		Vector3 position = transform.position;
		Vector3 followPosition = objectToFollow.transform.position + followOffset;
		position.x = followPosition.x;
		position.y = followPosition.y;
		transform.position = position;
	}

	private void OnPlayerDeath()
	{
		SnapToTarget();
	}
}
