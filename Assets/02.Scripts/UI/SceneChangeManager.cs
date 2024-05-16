using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : Singleton<SceneChangeManager>
{
    AsyncOperation operation;
    private float time;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ChangeToBusMap()
    {        
        StartCoroutine(LoadSceneAsyncCoroutine());
    }

    IEnumerator LoadSceneAsyncCoroutine()
    {
        UIManager.Instance.EnterBusMap();

        operation = SceneManager.LoadSceneAsync("Map");

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            time += Time.time;
            if (time > 5)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch(scene.name)
        {
            case "Map":
                UIManager.Instance.ChangeToBusMapMask();
                break;
        }
    }
}
