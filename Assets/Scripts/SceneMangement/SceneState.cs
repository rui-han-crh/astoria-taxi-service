using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Encapsulates the state of the scene that is loaded.
/// </summary>
public class SceneState
{
    /// <summary>
    /// The name of the scene entry point gameobject, tagged as Respawn.
    /// The player will be at the entry point after the scene loads.
    /// </summary>
    private readonly string sceneEntryPointName;
    public string SceneEntryPointName => sceneEntryPointName;

    /// <summary>
    /// The name of the scene that is loaded.
    /// </summary>
    private readonly string sceneName;
    public string SceneName => sceneName;

    public SceneState(string sceneEntryPointName, string sceneName)
    {
        this.sceneEntryPointName = sceneEntryPointName;
        this.sceneName = sceneName;
    }
}
