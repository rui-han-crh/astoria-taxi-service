using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles loading resources from the Resources folder.
/// 
/// Once loaded, resources are cached in a dictionary.
/// 
/// This class is a singleton and is not destroyed on scene load.
/// It is kept alive for the entire game.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    /// <summary>
    /// This dictionary mirrors the Resources folder structure.
    /// 
    /// The value mapped to may be either a folder, in which case it is represented as a string,
    /// or a file, in which case it is represented as a GameObject.
    /// </summary>
    private readonly Dictionary<string, object> resources = new Dictionary<string, object>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            return;
        }

        Destroy(gameObject);
        Debug.LogWarning("You tried to instantiate a second ResourceManager. Only one is allowed.");
    }

    public GameObject[] LoadAll<T>(string path) where T : Object
    {
        string[] splitPath = path.Split('/');

        Resources.LoadAll<T>(path)
            .ToList()
            .ForEach(resource => AddResource(splitPath, resource));

        try 
        {
            return ((Dictionary<string, object>)GetNestedValue(splitPath)).Values
                .Where(value => value is GameObject)
                .Select(value => (GameObject)value)
                .ToArray();
        }
        catch (System.Exception)
        {
            throw new System.Exception($"No resource found at path {path}");
        }
    }

    public GameObject Load<T>(string path) where T : Object
    {
        string[] splitPath = path.Split('/');

        var resource = Resources.Load<T>(path);

        AddResource(splitPath, resource);

        try
        {
            return (GameObject)GetNestedValue(splitPath);
        }
        catch (System.Exception)
        {
            throw new System.Exception($"No resource found at path {path}");
        }
    }

    private void AddResource(string[] keys, object resource)
    {
        var current = Instance.resources;

        // We exclude the last key because that is the name of the resource.
        // Every prior key maps string => Dictionary<string, object> representing a folder.
        // The last key maps the string to the resource itself.
        for (int i = 0; i < keys.Length - 1; i++)
        {
            if (!current.ContainsKey(keys[i]))
            {
                current[keys[i]] = new Dictionary<string, object>();
            }

            current = (Dictionary<string, object>)current[keys[i]];
        }

        current[keys.Last()] = resource;
    }

    private static object GetNestedValue(params string[] keys)
    {
        if (keys.Length == 0)
        {
            throw new System.Exception("No keys provided");
        }

        var current = Instance.resources;

        foreach (var key in keys)
        {
            if (!current.ContainsKey(key))
            {
                throw new System.Exception($"No resource found at path {string.Join("/", keys)}");
            }

            current = (Dictionary<string, object>)current[key];
        }

        return current;
    }
}
