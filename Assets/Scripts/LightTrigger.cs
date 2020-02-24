using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class LightTrigger : MonoBehaviour
{
	[Tooltip("If on, all components in 'components To Toggle Lights' need to be disabled to trigger the lights, else only one of the components need to become disabled to trigger lights")]
	public bool doesAllComponentsNeedToBeDisabled = true;
	public List<Light2D> lightsToTurnOff = new List<Light2D>();
	public List<Light2D> lightsToTurnOn = new List<Light2D>();
	public List<Behaviour> componentsToToggleLights = new List<Behaviour>();
    void Update()
    {
		if (ShouldTriggerLights())
		{
			for (int i = 0; i < lightsToTurnOff.Count; i++)
			{
				lightsToTurnOff[i].enabled = false;
			}
			for (int i = 0; i < lightsToTurnOn.Count; i++)
			{
				lightsToTurnOn[i].enabled = true;
			}
		}
    }
	private bool ShouldTriggerLights()
	{
		if (doesAllComponentsNeedToBeDisabled)
		{
			for (int i = 0; i < componentsToToggleLights.Count; i++)
			{
				if (componentsToToggleLights[i].enabled)
				{
					return false;
				}
			}
		}
		else
		{
			for (int i = 0; i < componentsToToggleLights.Count; i++)
			{
				if (!componentsToToggleLights[i].enabled)
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}
	public void TriggerLights()
	{
		for (int i = 0; i < lightsToTurnOff.Count; i++)
		{
			lightsToTurnOff[i].enabled = false;
		}
		for (int i = 0; i < lightsToTurnOn.Count; i++)
		{
			lightsToTurnOn[i].enabled = true;
		}
	}
}
