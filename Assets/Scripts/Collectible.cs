using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Collectible : MonoBehaviour
{
	public string animationName;
	[Space()]
	public bool isCollected = false;

	public static List<Collectible> list { get; private set; }
	private static bool isListDirty;

	Flowchart flowchart;

	private void Awake()
	{
		flowchart = GetComponent<Flowchart>();
		if (list == null)
		{
			list = new List<Collectible>(4);
		}
		list.Add(this);
		isListDirty = true;
	}

	private void Start()
	{
		if (isListDirty)
		{
			isListDirty = false;
			Debug.Log("Sort collectibles list");
			// Sort collectibles by order in hierarchy (or rather sibling index -- so it assumes all collectibles have the same parent)
			list.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
		}
	}

	private void OnDestroy()
	{
		list.Remove(this);
		isListDirty = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			Time.timeScale = 0;
			MemoryController.get.CollectMemory(this);
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
		isCollected = true;
	}
}
