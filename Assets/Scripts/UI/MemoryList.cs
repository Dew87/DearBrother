using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryList : MonoBehaviour
{
	public MemoryButton memoryPrefab;

	private void OnEnable()
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		if (MemoryController.get)
		{
			foreach (Collectible collectible in MemoryController.get.GetCollectedMemories())
			{
				MemoryButton button = Instantiate(memoryPrefab, transform);
				button.collectible = collectible;
			} 
		}
	}
}
