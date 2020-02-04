using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public GameObject objectToFollow;
    public Vector3 followOffset = new Vector3(0, 0, -10);
    private void Update()
    {
        transform.position = objectToFollow.transform.position + followOffset;
    }
}
