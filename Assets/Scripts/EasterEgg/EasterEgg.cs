using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EasterEgg : MonoBehaviour
{
	public RenderPipelineAsset rpa;

	// Start is called before the first frame update
	void Start()
	{
		GraphicsSettings.renderPipelineAsset = rpa;
	}

	private void OnDestroy()
	{
		GraphicsSettings.renderPipelineAsset = null;
	}
}
