using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	[Header("Follow player")]
	public Transform followOffsetTransform;
	public bool snapToPlayerOnStart = true;
	public PlayerController playerController;
	public float cameraSpeedWhenStill = 2f;
	public float cameraSpeedWhenGliding = 8f;
	public Extents bufferArea = new Extents(1, 2, 0.5f);
	[Tooltip("How far down the camera will be while gliding")]
	public float glidingOffset = 2f;

	[Header("Look-down")]
	public Transform lookTransform;
	public float lookDownDuration = 0.3f;
	public float lookDownDelay = 0.2f;
	public float lookDownDistance = 6f;
	public bool onlyLookWhenStill = true;

	public static PlayerCamera get { get; private set; }

	public enum Mode { FollowPlayer, Cinematic }
	public Mode mode { get; private set; }

	[HideInInspector] public bool useUnscaledTime = false;
	public Vector3 defaultFollowOffset { get; private set; }

	private Collider2D objectToFollowCollider;
	private LayerMask solidMask;
	private float lookDownTimer;
	private new Camera camera;

	private float baseSize;
	private float lookDownFactor;
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
		defaultFollowOffset = followOffsetTransform.localPosition;
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
		if (mode == Mode.FollowPlayer)
		{
			Vector2 playerVelocity = playerController.rb2d.velocity;
			FollowPlayer(playerVelocity);
			if (lookTransform)
			{
				LookDown(playerVelocity);
			}
		}

		camera.orthographicSize = baseSize / currentZoom;
	}

	public bool IsAtTarget()
	{
		return transform.position == playerController.transform.position;
	}

	public void SetZoom(float zoom, float duration)
	{
		IEnumerator Coroutine()
		{
			float t = 0;
			float startZoom = currentZoom;
			while (t <= 1)
			{
				currentZoom = Mathf.SmoothStep(startZoom, zoom, t);
				t += Time.deltaTime / duration;
				yield return null;
			}
		}

		StartCoroutine(Coroutine());
	}

	public void SetOffset(Vector3 offset, float duration)
	{
		IEnumerator Coroutine()
		{
			float t = 0;
			Vector3 startOffset = followOffsetTransform.localPosition;
			Vector3 step = (offset - startOffset) / duration;
			while (t <= 1)
			{
				followOffsetTransform.localPosition = VectorSmoothstep(startOffset, offset, t);
				t += Time.deltaTime / duration;
				yield return null;
			}
		}

		StartCoroutine(Coroutine());
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
			transform.position = VectorSmoothstep(startPosition, targetPosition, t);
			currentZoom = Mathf.Lerp(startZoom, zoom, t);
			t += Time.deltaTime / duration;
			yield return null;
		}
	}

	static Vector3 VectorSmoothstep(Vector3 from, Vector3 to, float t)
	{
		return new Vector3(Mathf.SmoothStep(from.x, to.x, t), Mathf.SmoothStep(from.y, to.y, t), Mathf.SmoothStep(from.z, to.z, t));
	}

	private void FollowPlayer(Vector2 playerVelocity)
	{
		Vector3 currentPosition = transform.position;
		Vector3 newCameraPosition = currentPosition;
		Vector3 followPosition = playerController.transform.position;
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
			if (playerController.currentState == playerController.glidingState)
			{
				followPosition.y -= glidingOffset;
				newCameraPosition.y = Mathf.MoveTowards(newCameraPosition.y, followPosition.y, cameraSpeedWhenGliding * Time.deltaTime);
			}
			else
			{
				newCameraPosition.y = Mathf.MoveTowards(newCameraPosition.y, followPosition.y, cameraSpeedWhenStill * Time.deltaTime);
			}
		}

		transform.position = newCameraPosition;
	}

	private void LookDown(Vector2 playerVelocity)
	{
		Vector3 newLookOffset = lookTransform.localPosition;

		bool grounded = playerController.CheckOverlaps(Vector2.down);

		bool isAbleToMove = onlyLookWhenStill ? Mathf.Approximately(playerVelocity.x, 0) : true;
		if (lookDownTimer >= lookDownDelay)
		{
			lookDownFactor += Time.deltaTime / lookDownDuration;
		}

		bool doLookDownTimer = playerController.isCrouchInputHeld && grounded && isAbleToMove;
		lookDownTimer = doLookDownTimer ? lookDownTimer + Time.deltaTime : 0;
		if (lookDownTimer <= 0 && newLookOffset.y < 0 && grounded)
		{
			lookDownFactor -= Time.deltaTime / lookDownDuration;
		}

		newLookOffset.y = Mathf.SmoothStep(0, -lookDownDistance, lookDownFactor);

		lookTransform.localPosition = newLookOffset;
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

	public IEnumerator MoveToTarget(float duration)
	{
		Vector3 position = transform.position;
		Vector3 startPosition = position;
		Vector3 followPosition = playerController.transform.position;
		float t = 0;
		while (t <= duration)
		{
			position.x = Mathf.SmoothStep(startPosition.x, followPosition.x, t / duration);
			position.y = Mathf.SmoothStep(startPosition.y, followPosition.y, t / duration); ;
			transform.position = position;
			t += Time.unscaledDeltaTime;
			yield return null;
		}
	}

	private void OnPlayerDeath()
	{
	}
}
