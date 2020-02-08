using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GrappleDetection : MonoBehaviour
{
    public float detectionRadius = 5f;
    public GameObject targetingCircle;

    public GrappleMotion playerGrappleMotion;
    public PlayerMovement playerMovement;

    private bool isFacingRight = true;
    private bool isSwinging = false;

    private LayerMask grappleLayer;
    private LayerMask solidLayer;
    private void OnDrawGizmos()
    {
        Handles.DrawWireArc(transform.position, Vector3.forward, isFacingRight ? Vector3.down : Vector3.up, 180f, detectionRadius);
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y - 5, transform.position.z), new Vector3(transform.position.x, transform.position.y + 5, transform.position.z));
    }
    private void Start()
    {
        grappleLayer = LayerMask.GetMask("GrapplePoint");
        solidLayer = LayerMask.GetMask("Solid");
    }
    private void Update()
    {
        float move = Input.GetAxis("Horizontal");
        if (move != 0)
        {
            isFacingRight = move > 0 ? true : false;
        }
        List<Collider2D> results = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, detectionRadius, grappleLayer));
        List<Collider2D> colliders = new List<Collider2D>();
        int length = results.Count;
        for (int i = 0; i < length; i++)
        {
            if (isFacingRight)
            {
                if (results[i].transform.position.x >= transform.position.x && !Physics2D.Linecast(transform.position, results[i].transform.position, solidLayer))
                {
                    colliders.Add(results[i]);
                }
            }
            else
            {
                if (results[i].transform.position.x <= transform.position.x && !Physics2D.Linecast(transform.position, results[i].transform.position, solidLayer))
                {
                    colliders.Add(results[i]);
                }
            }
        }
        int closestIndex = 0;
        for (int i = 0; i < colliders.Count; i++)
        {
            if (Vector2.Distance(transform.position, colliders[i].transform.position) < Vector2.Distance(transform.position, colliders[closestIndex].transform.position))
            {
                closestIndex = i;
            }
        }
        if (!isSwinging)
        {
            if (colliders.Count <= 0)
            {
                targetingCircle.SetActive(false);
            }
            else
            {
                targetingCircle.transform.position = colliders[closestIndex].transform.position;
                targetingCircle.SetActive(true);
            }
        }
        else
        {
            targetingCircle.transform.position = playerGrappleMotion.grapplePoint.transform.position;
        }
        if (colliders.Count > 0)
        {
            if (colliders[closestIndex] != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerGrappleMotion.grapplePoint = colliders[closestIndex].gameObject;
                    playerGrappleMotion.velocity = playerMovement.velocity;
                    playerGrappleMotion.enabled = true;
                    playerMovement.enabled = false;
                    isSwinging = true;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.E) && isSwinging)
        {
            playerMovement.velocity = playerGrappleMotion.velocity;
            playerGrappleMotion.enabled = false;
            playerMovement.enabled = true;
            isSwinging = false;
        }
    }
}
