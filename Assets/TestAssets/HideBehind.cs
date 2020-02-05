using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBehind : MonoBehaviour
{
    private Collider2D playerCollider;
    private LayerMask hideableMask;
    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        hideableMask = LayerMask.GetMask("Hideable");
    }
    private void Update()
    {

    }
    private bool CheckOverlap()
    {
        Collider2D hit = Physics2D.OverlapPoint(playerCollider.bounds.center, hideableMask);
        return hit != null;
    }
}
