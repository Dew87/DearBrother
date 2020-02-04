using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBalloon : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == playerMovement.gameObject)
        {
            playerMovement.balloonPower = true;
            Destroy(this.gameObject);
        }
    }
}
