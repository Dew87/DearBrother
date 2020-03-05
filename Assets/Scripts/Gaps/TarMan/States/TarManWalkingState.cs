using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TarManWalkingState : TarManState
{
	public float speed = 2f;
	public float acceleration = 20f;
	public float deceleration = 10f;
	public float tolerance = 0.2f;

	public override void Enter()
	{
		base.Enter();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		Vector2 position = tarMan.rb2d.position;
		Vector2 target = tarMan.pathPoints[tarMan.currentPositionInPath].position;

		float distance = Vector2.Distance(position, target);
		if (distance < tolerance)
		{
			tarMan.currentPositionInPath++;
			if (tarMan.currentPositionInPath == tarMan.pathPoints.Count)
			{
				tarMan.currentPositionInPath = 0;
				tarMan.TransitionState(tarMan.idleState);
			}
		}
		else
		{
			Vector2 direction = target - position;
			Vector2 velocity = tarMan.rb2d.velocity;

			velocity.x = Mathf.MoveTowards(velocity.x, speed * Mathf.Sign(direction.x), acceleration * Time.deltaTime);
			tarMan.rb2d.velocity = velocity;
		}
	}
}
