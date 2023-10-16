using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the data related to a house.
/// 
/// A house is commonly used as a drop off location for passengers.
/// </summary>
public class House : MonoBehaviour
{
    [SerializeField]
    private Transform doorTransform;

    public Transform DoorTransform => doorTransform;
}
