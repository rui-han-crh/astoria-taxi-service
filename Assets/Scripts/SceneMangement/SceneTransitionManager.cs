using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the scene entry transition.
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    void Start()
    {
        SceneState sceneState = SaveManager.GetSceneState();
        if (sceneState == null)
        {
            return;
        }

        if (sceneState.SceneName != null && SceneManager.GetActiveScene().name != sceneState.SceneName)
        {
            // Other scene should be active instead.
            SceneLoader.LoadScene(sceneState.SceneName);
        }

        if (sceneState.SceneEntryPointName != null)
        {
            GameObject[] sceneEntryPoints = GameObject.FindGameObjectsWithTag("Respawn");
            foreach (GameObject go in sceneEntryPoints) {
                if (go.name == sceneState.SceneEntryPointName)
                {
                    player.position = go.transform.position;
                    break;
                }
            }
        }
    }
}
