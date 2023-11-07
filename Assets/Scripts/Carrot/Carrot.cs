using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Horses))
        {
            collision.gameObject.GetComponent<CollisionEventEmitter>()
                .EmitTriggerEnter(action: (player) =>
                {
                    player.GetComponent<HungerManager>().IncreaseSatiation();
                });

            Destroy(gameObject);
        }
    }
}
