using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	public GameObject cam;
	public float parallaxStrength;
	public float parallaxStrengthY = 0;
	[Range(0, 1)]
	public float zoomInfluence = 0;
	public bool doParallax;
	public bool doRepeat = true;

	private float length;
	private Vector3 startpos;

    void Start()
    {
		if (doParallax)
		{
			startpos = transform.position;
		}
		else
		{
			transform.position = Vector3.zero;
		}
    }

    void LateUpdate()
    {
		if (doParallax)
		{
			length = GetComponent<SpriteRenderer>().bounds.size.x;
			float temp = cam.transform.position.x * (1 - parallaxStrength);
			float distance = cam.transform.position.x * parallaxStrength;
			float distanceY = cam.transform.position.y * parallaxStrengthY;

			if (doRepeat)
			{
				while (temp > startpos.x + length)
				{
					startpos.x += length;
				}
				while (temp < startpos.x - length)
				{
					startpos.x -= length;
				} 
			}
			transform.position = new Vector3(startpos.x + distance, startpos.y + distanceY, transform.position.z);

			if (PlayerCamera.get.currentZoom > 0)
			{
				float zoomRevert = Mathf.Lerp(1f / PlayerCamera.get.currentZoom, 1, zoomInfluence);
				transform.localScale = Vector2.one * zoomRevert; 
			}
		}
    }
}
