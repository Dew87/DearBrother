using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShakeConfig
{
	[Range(0, 1)]
	public float shakeIntensity;
	[Range(0, 1)]
	public float rotationalShakeIntensity;

	public ShakeConfig(float shakeIntensity = 0.5f, float rotationalShakeIntensity = 0.5f)
	{
		this.shakeIntensity = shakeIntensity;
		this.rotationalShakeIntensity = rotationalShakeIntensity;
	}
}

public class CameraShake : MonoBehaviour
{
	public float traumaDecrease = 0.2f;
	[Header("Translational shake")]
	public float frequency = 10f;
	public float maxAmplitude = 1f;
	[Header("Rotational shake")]
	public float rotationalFrequency = 10f;
	public float rotationalMaxAmplitude = 90f;

	public static CameraShake get { get; private set; }

	[Header("Debug")]
	[Range(0, 1)]
	[SerializeField] private float trauma;
	[Range(0, 1)]
	[SerializeField] private float rotationalTrauma;

	private void Awake()
	{
		get = this;
	}

	private void Update()
	{
		trauma -= Time.deltaTime * traumaDecrease;
		trauma = Mathf.Clamp(trauma, 0, 1);
		rotationalTrauma -= Time.deltaTime * traumaDecrease;
		rotationalTrauma = Mathf.Clamp(rotationalTrauma, 0, 1);
		float sqrTrauma = trauma * trauma;

		Vector3 position = transform.localPosition;
		position.x = GetRandomFloat(0, frequency) * sqrTrauma * maxAmplitude;
		position.y = GetRandomFloat(10, frequency) * sqrTrauma * maxAmplitude;
		transform.localPosition = position;

		transform.localRotation = Quaternion.Euler(0, 0, GetRandomFloat(20, frequency) * rotationalTrauma * rotationalTrauma * rotationalMaxAmplitude);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="intensity">0-1</param>
	/// <param name="rotationalIntensity">0-1</param>
	public void Shake(float intensity, float rotationalIntensity = 0)
	{
		Debug.Assert(intensity >= 0 && intensity <= 1, "Intensity must be between 0 and 1");
		Debug.Assert(rotationalIntensity >= 0 && rotationalIntensity <= 1, "Rotational intensity must be between 0 and 1");
		trauma += intensity;
		rotationalTrauma += rotationalIntensity;
	}

	public void Shake(ShakeConfig config)
	{
		Shake(config.shakeIntensity, config.rotationalShakeIntensity);
	}

	/// <summary>
	/// Returns smooth random number between -1 and 1
	/// </summary>
	/// <param name="seed"></param>
	public static float GetRandomFloat(float seed, float frequency)
	{
		return Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1;
	}
}
