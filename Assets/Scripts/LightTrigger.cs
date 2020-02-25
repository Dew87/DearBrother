using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class LightTrigger : MonoBehaviour
{
	[Tooltip("If on, all components in 'components To Toggle Lights' need to be disabled to trigger the lights, else only one of the components need to become disabled to trigger lights")]
	public bool doesAllComponentsNeedToBeDisabled = true;
	//[Tooltip("If on, all lights when toggled will light up with a speed depending on their start intensity, if not all will light up at the same speed")]
	//public bool doesLightUpEqually = true;
	public float lightUpSpeed = 1f;
	public List<Light2D> lightsToToggle = new List<Light2D>();
	public List<Behaviour> componentsToToggleLights = new List<Behaviour>();

	private Dictionary<Light2D, float> lightIntensities = new Dictionary<Light2D, float>();
	private bool isTriggered = false;
	private void Start()
	{
		for (int i = 0; i < lightsToToggle.Count; i++)
		{
			lightIntensities.Add(lightsToToggle[i], lightsToToggle[i].intensity);
		}
	}
	private void Update()
    {
		if (!isTriggered)
		{
			if (ShouldTriggerLights())
			{
				TriggerLights();
			}
		}
		else
		{ //get a check to see if all lights are lit up enough then disable this.
			for (int i = 0; i < lightsToToggle.Count; i++)
			{
				lightsToToggle[i].intensity = Mathf.MoveTowards(lightsToToggle[i].intensity, lightIntensities[lightsToToggle[i]], lightUpSpeed);
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
		for (int i = 0; i < lightsToToggle.Count; i++)
		{
			lightsToToggle[i].enabled = !lightsToToggle[i].enabled;
			isTriggered = true;
		}
	}
}
