using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys the game object if it is away from the player at a specific distance
/// depending on the time the game object has been alive.
///
/// As the game object gets older, the distance it can be away from the player
/// before it is destroyed decreases.
/// </summary>
public class AutoDestroy : MonoBehaviour
{
    private static readonly string PLAYER_TAG = "Player";
    private static readonly float STARTING_DISTANCE = 40f;
    private static readonly float MIN_DISTANCE = 20f;

    private Transform playerTransform;

    private float timeAlive = 0f;

    private void Awake()
    {
        // Find the player
        playerTransform = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;

        float maxDistance = Mathf.Max(STARTING_DISTANCE - timeAlive, MIN_DISTANCE);

        if (Vector3.Distance(transform.position, playerTransform.position) > maxDistance)
        {
            Destroy(gameObject.transform.root.gameObject);
        }
    }
}
