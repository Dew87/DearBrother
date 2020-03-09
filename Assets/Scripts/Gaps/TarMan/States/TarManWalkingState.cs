using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TarManWalkingState : TarManState
{
	public float speed = 2f;
	public float acceleration = 20f;
	public float deceleration = 10f;
	public float tolerance = 0.1f;

	private Vector2 velocity;

	public override void Enter()
	{
		base.Enter();

		velocity = Vector2.zero;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		Vector2 position = tarMan.transform.position;
		Vector2 target = tarMan.pathPoints[tarMan.currentPositionInPath].position;
		Vector2 direction = target - position;

		if (direction.magnitude < tolerance)
		{
			if (tarMan.currentPositionInPath == tarMan.pathPoints.Count - 1)
			{
				tarMan.TransitionState(tarMan.idleState);
			}
			else
			{
				tarMan.currentPositionInPath++;
			}
		}
		else
		{

			velocity = Vector2.Lerp(velocity, speed * direction.normalized, acceleration * Time.deltaTime);
			position += velocity * Time.deltaTime;
			tarMan.transform.position = position;
		}
	}

	public override void Exit()
	{
		base.Exit();

		velocity = Vector2.zero;
	}
}
