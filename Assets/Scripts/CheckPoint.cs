using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	public bool isActive;
	public TarManController currentTarMan;
	public Transform tarManCheckPoint;

	private static List<CheckPoint> CheckPointList = new List<CheckPoint>();

	public static CheckPoint GetActiveCheckPoint
	{
		get
		{
			if (CheckPointList != null)
			{
				foreach (CheckPoint checkPoint in CheckPointList)
				{
					if (checkPoint.isActive)
					{
						return checkPoint;
					}
				}
			}
			return null;
		}
	}

	public static Vector2 GetActiveCheckPointPosition
	{
		get
		{
			CheckPoint activeCheckPoint = GetActiveCheckPoint;
			if (activeCheckPoint != null)
			{
				return activeCheckPoint.transform.position;
			}
			return Vector2.zero;
		}
	}

	private void OnEnable()
	{
		isActive = false;
		CheckPointList.Add(this);
	}

	private void OnDisable()
	{
		isActive = false;
		CheckPointList.Remove(this);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isActive && collision.CompareTag("Player"))
		{
			ActivateCheckPoint();
		}
	}

	private void ActivateCheckPoint()
	{
		foreach (CheckPoint checkPoint in CheckPointList)
		{
			checkPoint.isActive = false;
		}

		isActive = true;

		EventManager.TriggerEvent("Checkpoint");
	}

	private void OnDrawGizmos()
	{
		Bounds bounds = GetComponent<Collider2D>().bounds;
		Color fillColor = isActive ? Color.cyan : Color.blue;
		fillColor.a = 0.5f;
		Gizmos.color = fillColor;
		Gizmos.DrawCube(bounds.center, bounds.size);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(transform.position, 0.1f);
	}
}
