using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PrefabGenerator : MonoBehaviour
{
    [SerializeField]
    protected int maxInstancesPerGeneration = 5;

    [SerializeField]
    protected float minCoolDown = 1f;

    [SerializeField]
    protected float maxCoolDown = 10f;

    protected Transform playerTransform;

    protected float timeToNextGeneration = 3f;

    protected virtual void Awake()
    {
        // Find the player
        playerTransform = GameObject.FindGameObjectWithTag(Tags.Player).transform;
    }

    private void Update()
    {
        if (timeToNextGeneration <= 0f)
        {
            Generate(Random.Range(1, maxInstancesPerGeneration), 20, 40);

            timeToNextGeneration = Random.Range(minCoolDown, maxCoolDown);
        }
        else
        {
            timeToNextGeneration -= Time.deltaTime;
        }
    }

    protected abstract void Generate(int numberOfInstances, float minDistance = 20, float maxDistance = 40);
}
