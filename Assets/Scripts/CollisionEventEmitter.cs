using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to emit collision events to any listeners.
/// </summary>
public class CollisionEventEmitter : MonoBehaviour
{
    public delegate void ActionOnPlayer(GameObject player);
    /// <summary>
    /// An event that requests the subscriber to perform an action on the player.
    /// </summary>
    public event Action<ActionOnPlayer> OnTriggerEnterAction;

    /// <summary>
    /// An event that gives the subscriber a game object to perform some action on.
    /// </summary>
    public event Action<GameObject> OnTriggerEnterGameObject;

    /// <summary>
    /// An event that gives the subscriber a game object to perform some action on.
    /// </summary>
    public event Action<GameObject> OnCollisionGameObject;

    public void EmitTriggerEnter(GameObject collidedGameObject = null, ActionOnPlayer action = null)
    {
        if (collidedGameObject != null)
        {
            OnTriggerEnterGameObject?.Invoke(collidedGameObject);
        }
        
        if (action != null)
        {
            OnTriggerEnterAction?.Invoke(action);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionGameObject?.Invoke(collision.gameObject);
    }
}
