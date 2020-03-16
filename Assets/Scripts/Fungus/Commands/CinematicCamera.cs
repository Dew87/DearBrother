using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Cinematic Camera", "")]
[AddComponentMenu("")]
public class CinematicCamera : Command
{
	public bool waitUntilFinished = true;
	[Space()]
	[Tooltip("Object to look at")]
	public Transform targetPositionTransform;
	[Tooltip("Position to look at. Only used if Target Position Transform is not set")]
	public Vector3 targetPosition;
	public float duration = 1;
	[Range(0, 1)]
	[Tooltip("Where between player and Target Position to look. Set to zero to only zoom without moving camera")]
	public float lerpFactor = 1;
	[Tooltip("Zoom multiplier")]
	public float zoom = 1;

	public override void OnEnter()
	{
		base.OnEnter();
		PlayerCamera.get.LookAtCinematically(GetPosition(), duration, lerpFactor, zoom);
		if (waitUntilFinished)
		{
			StartCoroutine(WaitAndContinue(duration));
		}
		else
		{
			Continue(); 
		}
	}

	public override string GetSummary()
	{
		string txt = "";
		if (lerpFactor != 0)
		{
			txt += "--> ";
			if (targetPositionTransform != null)
			{
				txt += targetPositionTransform.gameObject.name;
			}
			else
			{
				txt += targetPosition.ToString();
			}

			if (lerpFactor != 1)
			{
				txt += " * " + lerpFactor;
			} 
		}

		if (zoom != 1)
		{
			txt += ", zoom: " + zoom + "x";
		}

		txt += " in " + duration + "s";

		if (!waitUntilFinished)
		{
			txt += " (don't wait)";
		}

		return txt;
	}

	private IEnumerator WaitAndContinue(float duration)
	{
		yield return new WaitForSeconds(duration+0.1f);
		Continue();
	}

	private Vector3 GetPosition()
	{
		return targetPositionTransform != null ? targetPositionTransform.position : targetPosition;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		GizmosExt.DrawArrow(transform.position, GetPosition() - transform.position);
	}

	public override Color GetButtonColor()
	{
		return new Color32(216, 228, 170, 255);
	}
}
