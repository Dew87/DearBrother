using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineTest : MonoBehaviour
{
	private StateMachine stateMachine = new StateMachine();
	private State stateFirst;
	private State stateSecond;

	private void Start()
	{
		stateFirst = new State(stateFirstUpdate, stateFirstEnter);
		stateSecond = new State(stateSecondUpdate, null, stateSecondExit);
		stateMachine.Transition(stateFirst);
	}

	private void Update()
	{
		stateMachine.Update();
	}

	private void stateFirstEnter()
	{
		print("Enter first state!");
	}

	private void stateFirstUpdate()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			stateMachine.Transition(stateSecond);
		}
	}

	private void stateSecondUpdate()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			stateMachine.Transition(stateFirst);
		}
	}

	private void stateSecondExit()
	{
		print("Second state no more!");
	}
}
