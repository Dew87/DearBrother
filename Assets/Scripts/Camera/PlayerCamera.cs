using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	[Header("Follow player")]
	public Transform followOffsetTransform;
	public bool snapToPlayerOnStart = true;
	public PlayerController playerController;
	public float dampX = 0.95f;
	public float dampXInBuffer = 0.7f;
	public float dampY = 0.95f;
	public float dampYInBuffer = 0.7f;
	public float dieRestoreCameraDuration = 0.5f;
	public Extents bufferArea = new Extents(1, 2, 0.5f);
	[Tooltip("How far down the camera will be while gliding")]
	public float glidingOffset = 2f;

	public static PlayerCamera get { get; private set; }

	public enum Mode { FollowPlayer, Cinematic }
	public Mode mode { get; private set; }

	[HideInInspector] public bool useUnscaledTime = false;
	public Vector3 defaultFollowOffset { get; private set; }
	public float currentZoom { get; private set; } = 1;
	public float zoom2 { get; set; } = 1;

	private float deltaTime => useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

	private Collider2D objectToFollowCollider;
	private LayerMask solidMask;
	private float lookDownTimer;
	private new Camera camera;

	private float baseSize;
	private float lookDownFactor;

	private float zoomTimer = 0;
	private float zoomDuration;
	private float startZoom = 1;
	private float targetZoom = 1;

	private float offsetTimer = 0;
	private float offsetDuration;
	private Vector3 startOffset;
	private Vector3 targetOffset;

	private float savedZoom;
	private Vector3 savedOffset;

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
		defaultFollowOffset = followOffsetTransform.localPosition;
		savedOffset = defaultFollowOffset;
		savedZoom = 1;
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
	}

	private void OnEnable()
	{
		EventManager.StartListening("PlayerDeath", OnPlayerDeath);
		EventManager.StartListening("Checkpoint", OnCheckpoint);
	}

	private void OnDisable()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
		EventManager.StopListening("Checkpoint", OnCheckpoint);
	}

	private void Update()
	{
		if (mode == Mode.FollowPlayer)
		{
			Vector2 playerVelocity = playerController.rb2d.velocity;
			FollowPlayer(playerVelocity);
		}

		if (currentZoom != targetZoom && deltaTime > 0 && zoomDuration > 0)
		{
			currentZoom = Mathf.SmoothStep(startZoom, targetZoom, zoomTimer / zoomDuration);
			zoomTimer += deltaTime;
		}
		camera.orthographicSize = baseSize / (currentZoom * zoom2);

		if (followOffsetTransform.localPosition != targetOffset && deltaTime > 0 && offsetDuration > 0)
		{
			followOffsetTransform.localPosition = Util.VectorSmoothstep(startOffset, targetOffset, offsetTimer / offsetDuration);
			offsetTimer += deltaTime;
		}
	}

	public bool IsAtTarget()
	{
		return transform.position == playerController.transform.position;
	}

	public void SetZoom(float zoom, float duration)
	{
		if (targetZoom != zoom || zoomTimer > zoomDuration)
		{
			targetZoom = zoom;
			zoomTimer = 0;
			zoomDuration = duration;
			startZoom = currentZoom;
		}
	}

	public void SetOffset(Vector3 offset, float duration)
	{
		if (targetOffset != offset || offsetTimer > offsetDuration)
		{
			targetOffset = offset;
			offsetTimer = 0;
			offsetDuration = duration;
			startOffset = followOffsetTransform.localPosition;
		}
	}

	public void LookAtCinematically(Vector3 position, float transitionDuration, float factor = 1, float zoom = 1)
	{
		mode = Mode.Cinematic;
		if (zoom <= 0)
		{
			zoom = 0.0001f; // Avoid divide by zero
		}
		StartCoroutine(DoLookAtCinematically(transitionDuration, position, factor, zoom));
	}

	public void StopCinematic(float duration, bool resetZoom = true)
	{
		float zoom = resetZoom ? 1 : currentZoom;

		IEnumerator Coroutine()
		{
			yield return StartCoroutine(DoLookAtCinematically(duration, playerController.transform.position, 1, zoom));
			mode = Mode.FollowPlayer;
		}

		StartCoroutine(Coroutine());
	}

	private IEnumerator DoLookAtCinematically(float duration, Vector3 targetPosition, float factor, float zoom)
	{
		Vector3 startPosition = transform.position;
		float startZoom = currentZoom;

		targetPosition.z = 0;
		targetPosition = Vector3.Lerp(startPosition, targetPosition, factor);

		float t = 0;
		while (t <= 1)
		{
			if (mode != Mode.Cinematic) yield break;
			transform.position = Util.VectorSmoothstep(startPosition, targetPosition, t);
			currentZoom = Mathf.Lerp(startZoom, zoom, t);
			t += deltaTime / duration;
			yield return null;
		}
	}

	private void FollowPlayer(Vector2 playerVelocity)
	{
		Vector3 currentPosition = transform.position;
		Vector3 newCameraPosition = currentPosition;
		Vector3 followPosition = playerController.transform.position;

		if (playerController.currentState == playerController.glidingState)
		{
			followPosition.y -= glidingOffset;
		}

		if (followPosition.x < currentPosition.x + bufferArea.x && followPosition.x > currentPosition.x - bufferArea.x)
		{
			newCameraPosition.x = Util.DeltaTimedDamp(newCameraPosition.x, followPosition.x, dampXInBuffer, deltaTime);
		}
		else
		{
			newCameraPosition.x = Util.DeltaTimedDamp(newCameraPosition.x, followPosition.x, dampX, deltaTime);
		}

		if (followPosition.y < currentPosition.y + bufferArea.up && followPosition.y > currentPosition.y - bufferArea.down)
		{
			newCameraPosition.y = Util.DeltaTimedDamp(newCameraPosition.y, followPosition.y, dampYInBuffer, deltaTime);
		}
		else
		{
			newCameraPosition.y = Util.DeltaTimedDamp(newCameraPosition.y, followPosition.y, dampY, deltaTime);
		}

		transform.position = newCameraPosition;
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Vector3 position = transform.position;
			position.z = 0;
			Gizmos.DrawWireCube(position + bufferArea.localCenter, bufferArea.size);
		}
		else
		{
			if (playerController)
			{
				Gizmos.DrawWireCube(playerController.transform.position + bufferArea.localCenter, bufferArea.size);
			}
		}
	}

	public void SnapToTarget()
	{
		Vector3 position = transform.position;
		Vector3 followPosition = playerController.transform.position;
		position.x = followPosition.x;
		position.y = followPosition.y;
		transform.position = position;
	}

	private void OnPlayerDeath()
	{
		followOffsetTransform.localPosition = targetOffset = savedOffset;
		currentZoom = targetZoom = savedZoom;
		zoom2 = 1;
	}

	private void OnCheckpoint()
	{
		Debug.Log("Save camera vars");
		if (offsetDuration > 0)
		{
			savedOffset = targetOffset; 
		}
		if (zoomDuration > 0)
		{
			savedZoom = targetZoom; 
		}
	}
}
