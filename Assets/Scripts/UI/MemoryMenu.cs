using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MemoryMenu : SubMenu
{
	public MemoryButton memoryPrefab;
	public Transform memoryList;
	public GameObject returnButton;

	private List<MemoryButton> memoryButtons = new List<MemoryButton>();

	private void Start()
	{
		foreach (Transform child in memoryList)
		{
			Destroy(child.gameObject);
		}

		// Wait one frame before setting this up to let all Collectibles initialize
		StartCoroutine(NextFrame());

		IEnumerator NextFrame()
		{
			yield return null;
			List<Collectible> collectibleList = Collectible.list;
			memoryButtons.Capacity = collectibleList.Count;
			for (int i = 0; i < collectibleList.Count; i++)
			{
				MemoryButton memory = Instantiate(memoryPrefab, memoryList);
				memory.collectible = collectibleList[i];
				memoryButtons.Add(memory);
			}
		}
	}

	public override void Open()
	{
		base.Open();

		foreach (MemoryButton memory in memoryButtons)
		{
			memory.Refresh();
		}

		EventSystem.current.SetSelectedGameObject(returnButton);
		foreach (MemoryButton memory in memoryButtons)
		{
			if (memory.collectible.isCollected)
			{
				EventSystem.current.SetSelectedGameObject(memory.gameObject);
				break;
			}
		}
	}
}
