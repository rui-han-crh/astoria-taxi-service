using UnityEngine.SceneManagement;

/// <summary>
/// Manages loading of scenes.
/// </summary>
public class SceneLoader
{
    private static SceneLoader instance;
    public static SceneLoader Instance
    {
        get
        {
            instance ??= new SceneLoader();
            return instance;
        }
    }

    /// <summary>
    /// Loads a scene by name.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
