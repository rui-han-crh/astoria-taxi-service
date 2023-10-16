using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the scene transition when exiting a scene.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class SceneExit : MonoBehaviour
{
    /// <summary>
    /// The unique name of the entry position of the other scene, which is tagged as Respawn. 
    /// </summary>
    [SerializeField]
    private string otherSceneEntryPoint;

    /// <summary>
    /// The name of the other scene to load.
    /// </summary>
    [SerializeField]
    private string otherSceneName;


    // Transition to other scene when entering the trigger box.
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Update and save
        SceneState sceneState = new SceneState(otherSceneEntryPoint, otherSceneName);
        SaveManager.UpdateSceneState(sceneState);
        SaveManager.WriteToDisk();

        // Transition to other scene
        SceneLoader.LoadScene(otherSceneName);
    }
    
}
