using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Collectible : MonoBehaviour
{
	public string animationName;
	[Space()]
	public float collectAnimationDuration = 0.5f;
	public float collectAnimationGrowScale = 1.5f;
	public float collectZoom = 1.2f;
	[Space()]
	public bool isCollected = false;

	public static List<Collectible> list { get; private set; }
	private static bool isListDirty;

	private Flowchart flowchart;
	private SpriteRenderer spriteRenderer;
	private Animator memoryAnimator;

	private void Awake()
	{
		flowchart = GetComponent<Flowchart>();
		spriteRenderer = GetComponent<SpriteRenderer>();

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
			// Sort collectibles by order in hierarchy (or rather sibling index -- so it assumes all collectibles have the same parent)
			list.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
		}

		GameObject memoryCharacterGO = GameObject.Find("/Fungus/Characters/Memory");
		Character memoryCharacter = memoryCharacterGO == null ? null : memoryCharacterGO.GetComponent<Character>();
		memoryAnimator = GameObject.Find("/Fungus/Memory/NonDialogCanvas/Image").GetComponent<Animator>();

		if (memoryCharacter == null)
		{
			Debug.LogError("No Memory character found. Note: It's searched for by its absolute path.");
		}

		CustomSay[] commands = GetComponents<CustomSay>();
		foreach (CustomSay command in commands)
		{
			command.SetCharacter(memoryCharacter);
		}

		PlayAnimState[] animCommands = GetComponents<PlayAnimState>();
		foreach (PlayAnimState command in animCommands)
		{
			command.SetAnimator(memoryAnimator);
		}
	}

	private void OnDestroy()
	{
		list.Remove(this);
		isListDirty = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !PlayerController.get.IsInCutscene)
		{
			Time.timeScale = 0;
			PlayerController.get.Freeze(true, false);
			GetComponent<Collider2D>().enabled = false;
			MemoryController.get.CollectMemory(this);
			isCollected = true;
			StartCoroutine(DoCollect());
		}
	}

	private IEnumerator DoCollect()
	{
		Vector3 cameraNormalOffset = PlayerCamera.get.followOffsetTransform.transform.localPosition;
		float targetZoom = collectZoom / PlayerCamera.get.currentZoom;
		Vector3 targetOffset = transform.position - PlayerCamera.get.transform.position;
		targetOffset.z = 0;
		Color targetColor = new Color(1, 1, 1, 0);

		float t = 0;
		while (t < 1)
		{
			t += Time.unscaledDeltaTime / collectAnimationDuration;
			transform.localScale = Vector3.one * Mathf.Lerp(1, collectAnimationGrowScale, t);
			spriteRenderer.color = Color.Lerp(Color.white, targetColor, t);
			PlayerCamera.get.zoom2 = Mathf.SmoothStep(1, targetZoom, t);
			PlayerCamera.get.followOffsetTransform.localPosition = Util.VectorSmoothstep(cameraNormalOffset, targetOffset, t);
			yield return null;
		}

		yield return StartCoroutine(DoShowMemory());

		PlayerController.get.Freeze(false, false);
		Time.timeScale = 1;

		t = 0;
		while (t < 1)
		{
			t += Time.unscaledDeltaTime / collectAnimationDuration;
			PlayerCamera.get.zoom2 = Mathf.SmoothStep(targetZoom, 1, t);
			PlayerCamera.get.followOffsetTransform.localPosition = Util.VectorSmoothstep(targetOffset, cameraNormalOffset, t);
			yield return null;
		}

		GetComponent<SpriteRenderer>().enabled = false;
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
	}
}
