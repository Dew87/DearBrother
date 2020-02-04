using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBalloon : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            player.AddComponent<BalloonPower>();
            Destroy(this.gameObject);
        }
    }
}
