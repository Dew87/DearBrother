using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightintensity : MonoBehaviour
{
	[Range(0, 1)]
	public float alphaLow;
	[Range(0, 1)]
	public float alphaHigh;
	public float alternationTime;
	public SpriteRenderer spriteRenderer;

	private Color startColor;
	private bool isIncreasing;
	private float increaseSpeed;
    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
		startColor = spriteRenderer.color;
		spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alphaLow);
		isIncreasing = true;
		increaseSpeed = (alphaHigh - alphaLow) / alternationTime;
    }

    void Update()
    {
        if (isIncreasing)
		{
			if (spriteRenderer.color.a < alphaHigh)
			{
				spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, spriteRenderer.color.a + (Time.deltaTime * increaseSpeed));
			}
			else
			{
				spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alphaHigh);
				isIncreasing = false;
			}
		}
		else
		{
			if (spriteRenderer.color.a > alphaLow)
			{
				spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, spriteRenderer.color.a - (Time.deltaTime * increaseSpeed));
			}
			else
			{
				spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alphaLow);
				isIncreasing = true;
			}
		}
    }
}
