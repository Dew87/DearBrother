using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TarManAttackState : TarManState
{
	public float attackDelay = 0.5f;
	public float attackDuration = 1.0f;

	public GameObject attackObject;

	private float timer;

	public override void Enter()
	{
		base.Enter();

		timer = 0f;
		tarMan.animator.SetTrigger("Attack");
		tarMan.soundManager.TarManAttackSound();
	}

	public override void FixedUpdate()
	{
		timer += Time.deltaTime;
		if (timer > attackDuration)
		{
			tarMan.TransitionState(tarMan.previousState);
		}
		else if (timer > attackDelay)
		{
			attackObject.SetActive(true);
		}
	}

	public override void Exit()
	{
		base.Exit();

		attackObject.SetActive(false);
	}
}
