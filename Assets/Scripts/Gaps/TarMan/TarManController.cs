using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarManController : MonoBehaviour
{
	public List<Transform> pathPoints;
	public TarManSoundManager soundManager;

	[Header("States")]
	public TarManAttackState attackState;
	public TarManIdleState idleState;
	public TarManWalkingState walkingState;

	public TarManState currentState { get; private set; }
	public TarManState previousState { get; private set; }
	public int currentPositionInPath { get; set; }

	private int ClosestPathIndex
	{
		get
		{
			int closestIndex = 0;
			for (int i = 0; i < pathPoints.Count; i++)
			{
				if (Vector2.Distance(transform.position, pathPoints[i].position) < Vector2.Distance(transform.position, pathPoints[closestIndex].position))
				{
					closestIndex = i;
				}
			}
			return closestIndex;
		}
	}

	private IEnumerable<TarManState> IterateStates()
	{
		yield return attackState;
		yield return idleState;
		yield return walkingState;
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < pathPoints.Count; i++)
		{
			Gizmos.DrawWireSphere(pathPoints[i].position, 0.05f);
			if (i < pathPoints.Count - 1)
			{
				Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
			}
		}
	}

	private void Awake()
	{
		foreach (TarManState state in IterateStates())
		{
			state.Awake();
		}
	}

	private void Start()
	{
		foreach (TarManState state in IterateStates())
		{
			state.tarMan = this;
		}

		TransitionState(idleState);

		foreach (TarManState state in IterateStates())
		{
			currentState.Start();
		}

		currentPositionInPath = ClosestPathIndex;
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
		currentState.Update();
	}

	private void FixedUpdate()
	{
		currentState.FixedUpdate();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		currentState.OnTriggerEnter2D(collision);
	}

	public void TransitionState(TarManState newState)
	{
		if (currentState != null)
		{
			currentState.isCurrentState = false;
			currentState.Exit();
		}
		previousState = currentState;
		currentState = newState;
		if (currentState != null)
		{
			currentState.Enter();
			currentState.isCurrentState = true;
		}
	}

	private void OnPlayerDeath()
	{
		CheckPoint checkPoint = CheckPoint.GetActiveCheckPoint;
		if (checkPoint != null && checkPoint.currentTarMan == this && checkPoint.tarManCheckPoint != null)
		{
			transform.position = checkPoint.tarManCheckPoint.transform.position;
			currentPositionInPath = ClosestPathIndex;
			TransitionState(walkingState);
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

	private void OnValidate()
	{
		foreach (TarManState state in IterateStates())
		{
			state.OnValidate();
		}
	}
}
