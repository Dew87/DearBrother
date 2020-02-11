using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
	public float dropInitialSpeed = 0f;
	public float gravity = 51f;
	public float dropMaxSpeed = 10f;

	private const float castStartOffset = 0.02f;

	private Killer killer;
	private Rigidbody2D rb2d;
	private new Collider2D collider;

	private int castMask;
	private float speed;

	private StateMachine sm = new StateMachine();
	private State idleState;
	private State dropState;

	// Start is called before the first frame update
	private void Awake()
	{
		killer = GetComponent<Killer>();
		rb2d = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();
		castMask = LayerMask.GetMask("Solid", "Player");

		idleState = new State(IdleStateUpdate);
		dropState = new State(DropStateUpdate, DropStateEnter);
		sm.Transition(idleState);
	}

	private void OnEnable()
	{
		killer.onHitPlayer += OnHitPlayer;
	}

	private void OnDisable()
	{
		killer.onHitPlayer -= OnHitPlayer;
	}

	// Update is called once per frame
	private void Update()
	{
		sm.Update();
	}

	private void OnHitPlayer()
	{

	}

	private void IdleStateUpdate()
	{
		Bounds bounds = collider.bounds;
		RaycastHit2D hit = Physics2D.BoxCast(bounds.center + Vector3.down * castStartOffset, bounds.size, 0, Vector2.down, Mathf.Infinity, castMask);
		if (hit != false)
		{
			PlayerController player = hit.collider.GetComponentInParent<PlayerController>();
			if (player != null)
			{
				sm.Transition(dropState);
			}
		}
	}

	private void DropStateEnter()
	{
		speed = dropInitialSpeed;
	}

	private void DropStateUpdate()
	{
		speed += gravity * Time.deltaTime;
		rb2d.position += Vector2.down * speed * Time.deltaTime;
	}
}
