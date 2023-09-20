using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger
{
    public string Name { get; }
    public string ResourceFileReference { get; }

    public Passenger(
        string name,
        string resourceFileReference)
    {
        Name = name;
        ResourceFileReference = resourceFileReference;
    }
}
