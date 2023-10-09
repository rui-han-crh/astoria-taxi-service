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

    /// <summary>
    /// Returns all the resources at the given path from the cache.
    /// 
    /// If the path is a folder, returns all the resources in that folder.
    /// 
    /// This method will cache the resources in the dictionary if they are not already cached.
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T[] LoadAll<T>(string path) where T : Object
    {
        string[] splitPath = path.Split('/');

        try 
        {
            return ((Dictionary<string, object>)GetNestedValue(splitPath)).Values
                .Select(value => (T)value)
                .ToArray();
        }
        catch (System.Exception)
        {
            T[] lazyLoad = Resources.LoadAll<T>(path);

            foreach (T resource in lazyLoad)
            {
                AddResource(splitPath.Concat(new string[] { resource.name }).ToArray(), resource);
            }

            return lazyLoad;
        }
    }

    /// <summary>
    /// Loads the resource at the given path from the cache.
    /// 
    /// This is synonymous with Resources.Load<T>(path), except that it will
    /// cache the resource in the dictionary if it is not already cached.
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T Load<T>(string path) where T : Object
    {
        string[] splitPath = path.Split('/');

        try
        {
            print("Finding in cache");
            return (T)GetNestedValue(splitPath);
        }
        catch (System.Exception)
        {
            print("Not in cache, loading from resources");
            T resource = Resources.Load<T>(path);
            print(resources.Count);
            AddResource(splitPath, resource);
            return resource;
        }
    }

    private void AddResource(string[] keys, object resource)
    {
        var current = Instance.resources;
        print("The split path is " + string.Join(" ", keys));

        // We exclude the last key because that is the name of the resource.
        // Every prior key maps string => Dictionary<string, object> representing a folder.
        // The last key maps the string to the resource itself.
        for (int i = 0; i < keys.Length - 1; i++)
        {
            print(keys[i]);
            if (!current.ContainsKey(keys[i]))
            {
                print("Adding key " + keys[i] + "mapped to a new dictionary");
                current[keys[i]] = new Dictionary<string, object>();
            }

            current = (Dictionary<string, object>)current[keys[i]];
        }

        print("Adding key " + keys.Last() + " mapped to " + resource);
        current[keys.Last()] = resource;
    }

    private static object GetNestedValue(params string[] keys)
    {
        print(string.Join("/", keys));
        if (keys.Length == 0)
        {
            throw new System.Exception("No keys provided");
        }

        var current = Instance.resources;

        foreach (var key in keys)
        {
            if (!current.ContainsKey(key))
            {
                print("No resource found at path " + string.Join("/", keys));
                throw new System.Exception($"No resource found at path {string.Join("/", keys)}");
            }

            print("Found key: " + key);

            current = (Dictionary<string, object>)current[key];
        }

        return current;
    }
}
