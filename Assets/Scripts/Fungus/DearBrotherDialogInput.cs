// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.EventSystems;
using Fungus;

public enum ContinueMode
{
	Disabled,
	Enabled,
}

/// <summary>
/// Input handler for say dialogs.
/// </summary>
public class DearBrotherDialogInput : MonoBehaviour
{
	[Tooltip("Click to advance story")]
	[SerializeField] protected ContinueMode clickMode;

	[Tooltip("Delay between consecutive clicks. Useful to prevent accidentally clicking through story.")]
	[SerializeField] protected float nextClickDelay = 0f;

	[Tooltip("Allow holding Cancel to fast forward text")]
	[SerializeField] protected bool cancelEnabled = true;

	[Tooltip("Ignore input if a Menu dialog is currently active")]
	[SerializeField] protected bool ignoreMenuClicks = true;

	protected bool dialogClickedFlag;

	protected bool nextLineInputFlag;

	protected float ignoreClickTimer;

	protected StandaloneInputModule currentStandaloneInputModule;

	protected Writer writer;

	protected virtual void Awake()
	{
		writer = GetComponent<Writer>();
	}

	protected virtual void Update()
	{
		if (writer != null && writer.IsWriting)
		{
			if (clickMode == ContinueMode.Enabled)
			{
				if (Input.GetButtonDown("DialogContinue") || (cancelEnabled && Input.GetButton("DialogFastForward")))
				{
					SetNextLineFlag();
				}
			}
		}

		if (ignoreClickTimer > 0f)
		{
			ignoreClickTimer = Mathf.Max(ignoreClickTimer - Time.deltaTime, 0f);
		}

		if (ignoreMenuClicks)
		{
			// Ignore input events if a Menu is being displayed
			if (MenuDialog.ActiveMenuDialog != null &&
				MenuDialog.ActiveMenuDialog.IsActive() &&
				MenuDialog.ActiveMenuDialog.DisplayedOptionsCount > 0)
			{
				dialogClickedFlag = false;
				nextLineInputFlag = false;
			}
		}

		// Tell any listeners to move to the next line
		if (nextLineInputFlag)
		{
			var inputListeners = gameObject.GetComponentsInChildren<IDialogInputListener>();
			for (int i = 0; i < inputListeners.Length; i++)
			{
				var inputListener = inputListeners[i];
				inputListener.OnNextLineEvent();
			}
			nextLineInputFlag = false;
		}
	}

	#region Public members

	public ContinueMode ClickMode
	{
		get { return clickMode; }
		set { clickMode = value; }
	}

	/// <summary>
	/// Trigger next line input event from script.
	/// </summary>
	public virtual void SetNextLineFlag()
	{
		nextLineInputFlag = true;
	}

	/// <summary>
	/// Set the dialog clicked flag (usually from an Event Trigger component in the dialog UI).
	/// </summary>
	public virtual void SetDialogClickedFlag()
	{
		// Not in our game
	}

	/// <summary>
	/// Sets the button clicked flag.
	/// </summary>
	public virtual void SetButtonClickedFlag()
	{
		// No button in our game
	}

	#endregion
}

