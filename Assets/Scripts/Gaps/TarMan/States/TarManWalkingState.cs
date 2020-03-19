using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TarManWalkingState : TarManState
{
	public bool singleBlock;

	public float speed = 2f;
	public float acceleration = 20f;
	public float deceleration = 10f;
	public float distanceThreshold = 0.1f;

	private Vector2 velocity;

	public override void Enter()
	{
		base.Enter();

		if (tarMan.pathPoints.Count == 0)
		{
			tarMan.TransitionState(tarMan.idleState);
			return;
		}

		velocity = Vector2.zero;
		tarMan.animator.SetBool("Moving", true);
		tarMan.soundManager.PlayRepeat(tarMan.soundManager.walk);
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		Vector2 position = tarMan.transform.position;
		Vector2 target = tarMan.pathPoints[tarMan.currentPositionInPath].position;
		Vector2 direction = target - position;

		tarMan.FaceDirection(direction);

		if (direction.magnitude < distanceThreshold)
		{
			if (tarMan.currentPositionInPath == tarMan.pathPoints.Count - 1)
			{
				tarMan.TransitionState(tarMan.idleState);
			}
			else
			{
				tarMan.currentPositionInPath++;
				if (singleBlock)
				{
					tarMan.TransitionState(tarMan.idleState);
				}
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
		tarMan.animator.SetBool("Moving", false);
		tarMan.soundManager.StopSound();
	}
}
