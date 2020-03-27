using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBackAndForth : MonoBehaviour
{
	public float arcLength = 45;
	public float period = 0.3f;
	public float timeOffset;

	private float startRotation;

	private void Start()
	{
		startRotation = transform.eulerAngles.z;
	}

	void Update()
    {
		transform.rotation = Quaternion.Euler(0, 0, startRotation + Mathf.Sin((Time.time + timeOffset * period) * Mathf.PI * 2 / period) * 0.5f * arcLength);
	}
}
