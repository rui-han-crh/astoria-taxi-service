using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffBehaviour : MonoBehaviour
{
    private static readonly string CARRIAGE_BODY_TAG = "CarriageBody";
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(CARRIAGE_BODY_TAG))
        {
            collision.gameObject.GetComponent<CollisionEventEmitter>()
                .EmitTriggerEnter(action: (player) =>
                {
                    player.GetComponent<TripManager>().DropOff();
                });

            Destroy(gameObject);
        }
    }
}
