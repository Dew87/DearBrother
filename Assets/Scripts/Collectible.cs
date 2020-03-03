using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Collectible : MonoBehaviour
{
	public string animationName;
	[Space()]
	public bool collected = false;

	Flowchart flowchart;

	private void Awake()
	{
		flowchart = GetComponent<Flowchart>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			Time.timeScale = 0;
			ShowMemory();
		}
	}

	public void ShowMemory()
	{
		StartCoroutine(DoShowMemory());
	}

	private IEnumerator DoShowMemory()
	{
		yield return StartCoroutine(MemoryController.get.Open(animationName));
		
		flowchart.ExecuteBlock("Collected");
		while (flowchart.HasExecutingBlocks())
		{
			yield return null;
		}

		yield return StartCoroutine(MemoryController.get.Close());

		Time.timeScale = 1;
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
		collected = true;
	}
}
