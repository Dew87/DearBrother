using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlBehaviour : MonoBehaviour
{
	[Tooltip("Owl will start at sitpoint element 0, then when player gets RightSideLenght units to the rigth, the owl will teleport to next element in list until the last one.")]
	public List<Transform> sitPoints;
	public GameObject player;
	public float rightSideLength;

	private int currentSitPoint = 0;
	private void OnValidate()
	{
		Debug.Assert(sitPoints.Count > 0, "SitPoints list is empty, Did you forget to add sit points for owl?");
		Debug.Assert(player != null, "Player is empty, Did you forget to add player object to script?");
	}
	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(new Vector3(transform.position.x + rightSideLength, transform.position.y + 5, transform.position.z), new Vector3(transform.position.x + rightSideLength, transform.position.y - 5, transform.position.z));
	}
	private void Start()
	{
		currentSitPoint = 0;
		transform.position = sitPoints[currentSitPoint].position;
		transform.rotation = sitPoints[currentSitPoint].rotation;
	}
	private void Update()
    {
        if (player.transform.position.x > transform.position.x + rightSideLength && currentSitPoint  + 1 < sitPoints.Count)
		{
			currentSitPoint++;
			transform.position = sitPoints[currentSitPoint].position;
			transform.rotation = sitPoints[currentSitPoint].rotation;
		}
    }
}
