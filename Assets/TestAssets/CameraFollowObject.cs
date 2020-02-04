using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public GameObject objectToFollow;
    public Vector2 followOffset;
    private void Update()
    {
        transform.position = objectToFollow.transform.position + (Vector3)followOffset;
    }
}
