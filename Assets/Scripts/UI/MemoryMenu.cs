using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MemoryMenu : MonoBehaviour
{
	public MemoryButton memoryPrefab;
	public Transform memoryList;
	public GameObject returnButton;

	private List<MemoryButton> memoryButtons = new List<MemoryButton>();
	private CanvasGroup canvasGroup;

	private void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();

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

	private void OnEnable()
	{
		GetComponent<SubMenu>().onOpen += OnOpen;
	}

	private void OnDisable()
	{
		GetComponent<SubMenu>().onOpen -= OnOpen;
	}

	protected void Update()
	{
		canvasGroup.interactable = !MemoryController.isOpen;
	}

	protected void OnOpen()
	{
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
