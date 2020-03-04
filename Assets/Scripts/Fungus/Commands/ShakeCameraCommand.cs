using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Shake Camera", "")]
[AddComponentMenu("")]
public class ShakeCameraCommand : Command
{
	[Range(0, 1)] public float intensity;
	[Range(0, 1)] public float rotationalIntensity;

	public override void OnEnter()
	{
		base.OnEnter();

		CameraShake.get.Shake(intensity, rotationalIntensity);
		Continue();
	}

	public override string GetSummary()
	{
		return "Intensity: " + intensity + ", Rotational: " + rotationalIntensity;
	}

	public override Color GetButtonColor()
	{
		return new Color32(216, 228, 170, 255);
	}
}
