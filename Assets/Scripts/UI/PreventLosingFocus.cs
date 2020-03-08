using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/* Either me or Unity is dumb, I should not have to do this
 * It seems like even with the Graphic Raycaster disabled,
 * clicking outside any buttons will still make them lose focus.
 * This is a workaround for that.
 */

public class PreventLosingFocus : MonoBehaviour
{
	GameObject lastSelected;

	// Start is called before the first frame update
	void Start()
	{
		lastSelected = EventSystem.current.currentSelectedGameObject;
	}

	// Update is called once per frame
	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			EventSystem.current.SetSelectedGameObject(lastSelected);
		}

		lastSelected = EventSystem.current.currentSelectedGameObject;
	}
}
