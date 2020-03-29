using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolableBehaviour
{
	[Range(0, 360f)]
	[Tooltip("Degrees, 0 = right, 90 = up, etc.")]
	public float direction = 270f;
	public float speed = 3f;

	private Rigidbody2D rb2d;
	private int solidMask;

	private void Awake()
	{
		solidMask = LayerMask.GetMask("Solid", "SolidNoBlockGrapple");
		rb2d = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		EventManager.StartListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnDisable()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
	}

	private void FixedUpdate()
	{
		float angle = direction * Mathf.Deg2Rad;
		Vector2 directionVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		rb2d.position += directionVector * speed * Time.deltaTime;
	}

	public bool FastForward(float time)
	{
		float angle = direction * Mathf.Deg2Rad;
		Vector2 directionVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		float positionOffset = speed * time;

		if (Physics2D.Raycast(transform.position, directionVector, positionOffset, solidMask))
		{
			Die();
			return false;
		}
		else
		{
			Vector2 move = positionOffset * directionVector;
			transform.position += new Vector3(move.x, move.y, 0);
			return true;
		}

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Solid"))
		{
			Die();
		}
	}

	private void OnPlayerDeath()
	{
		Die();
	}
}
