using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffBehaviour : MonoBehaviour
{
    private static readonly string PLAYER_TAG = "Player";
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG))
        {
            collision.gameObject.GetComponent<TripManager>().DropOff();

            Destroy(gameObject);
        }
    }
}
