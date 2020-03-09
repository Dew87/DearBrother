using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingCircleBehaviour : MonoBehaviour
{
	public float startSize = 1f;
	public float endSize = 0.8f;
	public float scaleSpeed = 1f;
	public float rotationSpeed = 90f;

	private void OnDisable()
	{
		gameObject.transform.localScale = new Vector3(startSize, startSize, startSize);
	}

	private void Update()
	{
		if (gameObject.transform.localScale.x > endSize)
		{
			gameObject.transform.localScale = Vector3.MoveTowards(gameObject.transform.localScale, new Vector3(endSize, endSize, endSize), Time.deltaTime * scaleSpeed);
		}
		gameObject.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
	}
}
