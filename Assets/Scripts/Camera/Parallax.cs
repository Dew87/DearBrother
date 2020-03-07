using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	public GameObject cam;
	public float parallaxStrength;
	public bool doParallax;

	private float length, startpos;
    void Start()
    {
		if (doParallax)
		{
			startpos = transform.position.x;
			length = GetComponent<SpriteRenderer>().bounds.size.x;
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
			float temp = cam.transform.position.x * (1 - parallaxStrength);
			float distance = cam.transform.position.x * parallaxStrength;

			if (temp > startpos + length)
			{
				startpos += length;
			}
			else if (temp < startpos - length)
			{
				startpos -= length;
			}
			transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);
		}
    }
}
