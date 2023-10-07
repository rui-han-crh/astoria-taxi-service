using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Represents a TaxiPoint by it's (parent's) unique name and scene.
/// Also for the serialization of a TaxiPoint when it is a destination in another scene.
/// </summary>
public class TaxiPointLocation
{
    private string taxiPointName;
    private string taxiPointSceneName;
    public string TaxiPointName => taxiPointName;
    public string TaxiPointSceneName => taxiPointSceneName;

    public TaxiPointLocation(string taxiPointName, string taxiPointSceneName)
    {
        this.taxiPointName = taxiPointName;
        this.taxiPointSceneName = taxiPointSceneName;
    }
    public TaxiPointLocation(TaxiPoint taxiPoint)
    {
        taxiPointName = taxiPoint.transform.parent.name;
        taxiPointSceneName = SceneManager.GetActiveScene().name;
    }

    // Parameterless constructor for deserializing.
    public TaxiPointLocation()
    {
        taxiPointName = null;
        taxiPointSceneName = null;
    }
    public bool isSameTaxiPoint(TaxiPoint taxiPoint)
    {
        return taxiPoint.transform.parent.name == taxiPointName && SceneManager.GetActiveScene().name == taxiPointSceneName;
    }
}
