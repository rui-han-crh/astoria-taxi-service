using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles loading resources from the Resources folder.
/// 
/// Once loaded, resources are cached in a dictionary.
/// 
/// This class behaves singleton and is never destroyed for the entire lifecycle of the program.
/// </summary>
public static class ResourceManager
{

    /// <summary>
    /// This dictionary mirrors the Resources folder structure.
    /// 
    /// The value mapped to may be either a folder, in which case it is represented as a string,
    /// or a file, in which case it is represented as a GameObject.
    /// </summary>
    private static readonly Dictionary<string, object> resources = new Dictionary<string, object>();

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
    public static T[] LoadAll<T>(string path) where T : Object
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
    public static T Load<T>(string path) where T : Object
    {
        string[] splitPath = path.Split('/');

        try
        {
            T foundValue = (T)GetNestedValue(splitPath);
            return foundValue;
        }
        catch (System.Exception)
        {
            T resource = Resources.Load<T>(path);
            AddResource(splitPath, resource);
            return resource;
        }
    }

    private static void AddResource(string[] keys, object resource)
    {
        var current = resources;

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

        var current = resources;

        for (int i = 0; i < keys.Length - 1; i++)
        {
            if (!current.ContainsKey(keys[i]))
            {
                throw new System.Exception($"No resource found at path {string.Join("/", keys)}");
            }

            current = (Dictionary<string, object>)current[keys[i]];
        }

        return current[keys.Last()];
    }
}
