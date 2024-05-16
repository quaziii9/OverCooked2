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
        //UIManager.Instance.EnterBusMap();
        //Debug.Log(UIManager.Instance.maskInEnd);
        //if(UIManager.Instance.maskInEnd == true) Invoke("LoadSceneAsyncCoroutine", 2F);
       StartCoroutine(LoadSceneAsyncCoroutine());

        
    }

    IEnumerator LoadSceneAsyncCoroutine()
    {
        operation = SceneManager.LoadSceneAsync("Map");


        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {

            if (operation.progress >= 0.9f)
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
