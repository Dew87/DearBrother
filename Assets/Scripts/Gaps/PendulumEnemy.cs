using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumEnemy : MonoBehaviour
{
    [Tooltip("Degrees")]
    [Range(0, 360f)]
    public float arcLength = 150;
    [Tooltip("Duration for one cycle (seconds)")]
    public float period = 1f;
    [Tooltip("Offset from global timer (seconds), to make different pendulums start at different positions in cycle.")]
    public float cycleOffset = 0;

    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb2d.rotation = Mathf.Sin((Time.fixedTime + cycleOffset) * Mathf.PI * 2 / period) * 0.5f * arcLength;
    }

    private void OnDrawGizmos()
    {
        if (transform.GetChild(0))
        {
            float distance = Vector2.Distance(transform.position, transform.GetChild(0).position);
            float theta = (-90 + arcLength * 0.5f) * Mathf.Deg2Rad;
            Vector2 down = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
            UnityEditor.Handles.DrawWireArc(transform.position, Vector3.back, down, arcLength, distance); 
        }
    }

    private void OnValidate()
    {
        float initialRotation = Mathf.Sin(cycleOffset * Mathf.PI * 2 / period) * 0.5f * arcLength;
        transform.rotation = Quaternion.Euler(0, 0, initialRotation);
    }
}
