using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReceiver : MonoBehaviour
{
	public string testEvent = "TestEvent";

	private void OnDisable()
	{
		EventManager.StopListening(testEvent, OnTestEvent);
	}

	private void OnEnable()
	{
		EventManager.StartListening(testEvent, OnTestEvent);
	}

	private void OnTestEvent()
	{
		Debug.Log(testEvent + " received by " + this.ToString());
	}
}
