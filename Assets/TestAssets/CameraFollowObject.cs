using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public GameObject objectToFollow;
    public Vector3 followOffset = new Vector3(0, 0, -10);
    public float cameraFollowSpeed = 0.1f;
    public bool isStuckOnObject = false;
    public bool hasAcceleration = true;
    private void Update()
    {
        if (isStuckOnObject)
        {
            transform.position = objectToFollow.transform.position + followOffset;
        }
        else
        {
            float followSpeed;
            if (hasAcceleration)
            {
                followSpeed = Vector3.Distance(transform.position, objectToFollow.transform.position + followOffset) * cameraFollowSpeed * Time.deltaTime;
            }
            else
            {
                followSpeed = cameraFollowSpeed * Time.deltaTime;
            }
            transform.position = Vector3.MoveTowards(transform.position, (objectToFollow.transform.position + followOffset), followSpeed);
        }
    }
}
