using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ForegroundObject : MonoBehaviour
{
    private float fadeAlpha;
    private SpriteRenderer spriteRenderer;
	private Tilemap tilemap;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
		tilemap = GetComponent<Tilemap>();
    }

    public void SetFadeAlpha(float alpha)
    {
		if (spriteRenderer)
		{
			Color color = spriteRenderer.color;
			color.a = alpha;
			spriteRenderer.color = color; 
		}

		if (tilemap)
		{
			Color color = tilemap.color;
			color.a = alpha;
			tilemap.color = color;
		}
    }
}
