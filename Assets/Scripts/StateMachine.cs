using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct State
{
	public Action update;
	public Action enter;
	public Action exit;

	public State(Action update = null, Action enter = null, Action exit = null)
	{
		this.update = update;
		this.enter = enter;
		this.exit = exit;
	}
}

public class StateMachine
{
	public State currentState { get; private set; }

	public void Transition(State state)
	{
		currentState.exit?.Invoke();
		currentState = state;
		currentState.enter?.Invoke();
	}

	public void Update()
	{
		currentState.update?.Invoke();
	}
}
