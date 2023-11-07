using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.CarriageBody))
        {
            collision.gameObject.GetComponent<CollisionEventEmitter>()
                .EmitTriggerEnter(collidedGameObject: gameObject);
        }
    }
}
