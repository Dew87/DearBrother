using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryList : MonoBehaviour
{
	public MemoryButton memoryPrefab;

	private List<MemoryButton> memories = new List<MemoryButton>();

	private void Start()
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		// Wait one frame before setting this up to let all Collectibles initialize
		StartCoroutine(NextFrame());

		IEnumerator NextFrame()
		{
			yield return null;
			List<Collectible> collectibleList = Collectible.list;
			memories.Capacity = collectibleList.Count;
			for (int i = 0; i < collectibleList.Count; i++)
			{
				MemoryButton memory = Instantiate(memoryPrefab, transform);
				memory.collectible = collectibleList[i];
				memories.Add(memory);
			}
		}

	}

	private void OnEnable()
	{
		foreach (MemoryButton memory in memories)
		{
			memory.Refresh();
		}
	}
}
