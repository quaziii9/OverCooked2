using UnityEngine;
using UnityEngine.SceneManagement;

public static class PerformBootstrap
{
    const string SceneName = "BootstrapScene";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        // traverse the currently loaded scenes
        for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; ++sceneIndex)
        {
            var candidate = SceneManager.GetSceneAt(sceneIndex);

            // early out if already loaded
            if (candidate.name == SceneName)
                return;
        }

        Debug.Log("Loading Bootstrap scene: " + SceneName);

        // additively load the bootstrap scene
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }
}

public class BootstrappedData : Singleton<BootstrappedData>
{
    public void Test()
    {
        Debug.Log("Bootstrap is working!");
    }
}