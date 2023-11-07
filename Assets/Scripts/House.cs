using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Stores the data related to a house.
/// 
/// A house is commonly used as a drop off location for passengers.
/// </summary>
public class House : MonoBehaviour
{
    private static readonly Dictionary<string, House> houseMap = new Dictionary<string, House>();
    private static readonly List<House> houseList = new List<House>();

    [SerializeField]
    private Transform doorTransform;

    public Transform DoorTransform => doorTransform;

    private void Awake()
    {
        if (!houseMap.ContainsKey(UniqueKey))
        {
            houseMap.Add(UniqueKey, this);
            houseList.Add(this);
        }
    }

    public string UniqueKey { get => name + " " + doorTransform.name;}

    public static House FindByName(string name)
    {
        return houseMap[name];
    }

    public static House GetRandomHouse()
    {
        return houseList[Random.Range(0, houseList.Count)];
    }
}
