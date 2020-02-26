using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	[Header("Follow player")]
	public bool snapToPlayerOnStart = true;
	public PlayerController playerController;
	public Vector3 followOffset;
	public float cameraSpeedWhenStill = 2f;
	public Extents bufferArea = new Extents(1, 2, 0.5f);

	[Header("Look-down")]
	public Transform lookTransform;
	public float lookDownSpeed = 20f;
	public float lookDownDelay = 0.2f;
	public float lookDownDistance = 6f;
	public bool onlyLookWhenStill = true;

	public static PlayerCamera get { get; private set; }

	private Collider2D objectToFollowCollider;
	private LayerMask solidMask;
	private float lookDownTimer;
	private new Camera camera;

	private float baseSize;
	private Vector3 playerFollowPosition;
	private Vector3 cinematicPosition;
	private float cinematicLerp;
	private float currentZoom = 1;

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

	private void Awake()
	{
		get = this;
	}

	private void Start()
	{
		camera = GetComponentInChildren<Camera>();
		baseSize = camera.orthographicSize;
		if (snapToPlayerOnStart)
		{
			SnapToTarget();
		}
		solidMask = LayerMask.GetMask("Solid");

		if (lookTransform == null)
		{
			Debug.LogError("Camera's Look Transform is not assigned. Are you using the camera prefab?");
		}
	}

	private void OnEnable()
	{
		EventManager.StartListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnDisable()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
	}

	private void Update()
	{
		Vector2 playerVelocity = playerController.rb2d.velocity;
		FollowPlayer(playerVelocity);
		if (lookTransform)
		{
			LookDown(playerVelocity); 
		}

		transform.position = Vector3.Lerp(playerFollowPosition, cinematicPosition, cinematicLerp);
		camera.orthographicSize = baseSize / currentZoom;
	}

	public void LookAtCinematically(Vector3 position, float transitionDuration, float factor = 1, float zoom = 1)
	{
		cinematicPosition = position;
		if (zoom <= 0)
		{
			zoom = 0.0001f; // Avoid divide by zero
		}
		StartCoroutine(DoLookAtCinematically(transitionDuration, factor, zoom));
	}

	private IEnumerator DoLookAtCinematically(float duration, float lerp, float zoom)
	{
		float startLerp = cinematicLerp;
		float startZoom = currentZoom;
		float t = 0;
		while (t <= 1)
		{
			cinematicLerp = Mathf.Lerp(startLerp, lerp, t);
			currentZoom = Mathf.Lerp(startZoom, zoom, t);
			t += Time.deltaTime / duration;
			yield return null;
		}
	}

	private void FollowPlayer(Vector2 playerVelocity)
	{
		Vector3 currentPosition = playerFollowPosition;
		Vector3 newCameraPosition = currentPosition;
		Vector3 followPosition = playerController.transform.position + followOffset;
		if (Mathf.Abs(playerVelocity.x) >= cameraSpeedWhenStill)
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

		if (Mathf.Abs(playerVelocity.y) > cameraSpeedWhenStill)
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

		playerFollowPosition = newCameraPosition;
	}

	private void LookDown(Vector2 playerVelocity)
	{
		Vector3 newLookOffset = lookTransform.localPosition;

		bool grounded = playerController.CheckOverlaps(Vector2.down);

		bool isAbleToMove = onlyLookWhenStill ? Mathf.Approximately(playerVelocity.x, 0) : true;
		if (lookDownTimer >= lookDownDelay)
		{
			newLookOffset.y = Mathf.MoveTowards(newLookOffset.y, -lookDownDistance, lookDownSpeed * Time.deltaTime);
		}

		bool doLookDownTimer = (playerController.currentState == playerController.crouchingState || playerController.currentState == playerController.crawlingState) && grounded && isAbleToMove;
		lookDownTimer = doLookDownTimer ? lookDownTimer + Time.deltaTime : 0;
		if (lookDownTimer <= 0 && newLookOffset.y < 0 && grounded)
		{
			Debug.Log("Moving back");
			newLookOffset.y = Mathf.MoveTowards(newLookOffset.y, 0, lookDownSpeed * Time.deltaTime);
		}

		lookTransform.localPosition = newLookOffset;
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
			Gizmos.DrawWireCube(playerController.transform.position + bufferArea.localCenter, bufferArea.size);
		}
	}

	public void SnapToTarget()
	{
		Vector3 position = transform.position;
		Vector3 followPosition = playerController.transform.position + followOffset;
		position.x = followPosition.x;
		position.y = followPosition.y;
		playerFollowPosition = position;
		transform.position = position;
	}

	private void OnPlayerDeath()
	{
	}
}
