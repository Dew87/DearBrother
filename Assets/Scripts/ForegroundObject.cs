using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundObject : MonoBehaviour
{
    private float fadeAlpha;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetFadeAlpha(float alpha)
    {
        fadeAlpha = alpha;
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
