using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
	private Dictionary<string, UnityEvent> eventDictionary;
	private static EventManager instance;

	private static EventManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType(typeof(EventManager)) as EventManager;

				if (instance == null)
				{
					Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
				}
				else
				{
					instance.Initialise();
				}
			}
			return instance;
		}
	}

	public static void StartListening(string eventName, UnityAction listener)
	{
		UnityEvent thisEvent;
		if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.AddListener(listener);
		}
		else
		{
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			Instance.eventDictionary.Add(eventName, thisEvent);
		}
	}

	public static void StopListening(string eventName, UnityAction listener)
	{
		if (instance == null)
		{
			// Removes Debug.LogError if EventManager is removed before the listener
			return;
		}
		UnityEvent thisEvent;
		if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.RemoveListener(listener);
		}
	}

	public static void TriggerEvent(string eventName)
	{
		UnityEvent thisEvent;
		if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.Invoke();
		}
	}

	private void Initialise()
	{
		if (eventDictionary == null)
		{
			eventDictionary = new Dictionary<string, UnityEvent>();
		}
	}
}
