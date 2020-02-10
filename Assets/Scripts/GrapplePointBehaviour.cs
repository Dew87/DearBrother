using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePointBehaviour : MonoBehaviour
{
    public enum GrappleType
    {
        Swing,
        Whip,
        Pull
    }
    public GrappleType grappleType;
    public void UseGrapple()
    {
        if (grappleType == GrappleType.Whip)
        {
            Destroy(gameObject);
        }
    }
}
