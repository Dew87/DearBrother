using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Collectable : MonoBehaviour
{
	public string animationName;

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
			StartCoroutine(DoCollect());
		}
	}

	private IEnumerator DoCollect()
	{
		yield return StartCoroutine(MemoryController.get.Open(animationName));
		
		flowchart.ExecuteBlock("Collected");
		while (flowchart.HasExecutingBlocks())
		{
			yield return null;
		}

		yield return StartCoroutine(MemoryController.get.Close());

		Time.timeScale = 1;
		gameObject.SetActive(false);
	}
}
