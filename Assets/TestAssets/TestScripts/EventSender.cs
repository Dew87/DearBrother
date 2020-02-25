using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSender : MonoBehaviour
{
	public string testEvent = "TestEvent";
	public KeyCode eventKey = KeyCode.Q;

	private void Update()
	{
		if (Input.GetKeyDown(eventKey))
		{
			Debug.Log(testEvent + " sent by " + this.ToString());
			EventManager.TriggerEvent(testEvent);
		}
	}
}
