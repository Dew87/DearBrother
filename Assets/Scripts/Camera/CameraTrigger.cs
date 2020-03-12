using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
	public bool reset = false;
	public bool changeOffset = true;
	public bool changeZoom = true;
	[Space()]
	public Vector3 newOffset;
	public float zoom = 1;
	public float duration = 0.5f;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (reset)
			{
				if (changeOffset)
				{
					PlayerCamera.get.SetOffset(PlayerCamera.get.defaultFollowOffset, duration); 
				}
				if (changeZoom)
				{
					PlayerCamera.get.SetZoom(1, duration); 
				}
			}
			else
			{
				if (changeOffset)
				{
					PlayerCamera.get.SetOffset(newOffset, duration);
				}
				if (changeZoom)
				{
					PlayerCamera.get.SetZoom(zoom, duration);
				}
			}
		}
	}

	private void OnValidate()
	{
		if (zoom <= 0)
		{
			zoom = 0.001f;
		}
	}
}
