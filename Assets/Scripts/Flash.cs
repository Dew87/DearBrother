using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
	public float decreaseDuration = 1f;
	public float increaseDuration = 0.5f;

	private Graphic graphic;
	private float alpha = 1;

	private bool increase = false;

	// Start is called before the first frame update
	void Start()
	{
		graphic = GetComponent<Graphic>();
	}

	// Update is called once per frame
	void Update()
	{
		Color color = graphic.color;
		color.a = alpha;
		graphic.color = color;

		if (increase)
		{
			alpha += Time.deltaTime / increaseDuration;
			if (alpha >= 1)
			{
				increase = false;
			}
		}
		else
		{
			alpha -= Time.deltaTime / decreaseDuration;
			if (alpha <= 0)
			{
				increase = true;
			}
		}
	}
}
