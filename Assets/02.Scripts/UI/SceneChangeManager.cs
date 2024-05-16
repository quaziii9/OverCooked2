using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class SceneChangeManager : Singleton<SceneChangeManager>
{
    AsyncOperation operation;
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ChangeToBusMap()
    {
        SoundManager.Instance.FadeInAudio(SoundManager.Instance.bgmChangeAudioSource, 0, "BusMap");
        SoundManager.Instance.FadeOutAudio(SoundManager.Instance.bgmAudioSource, 0);
        StartCoroutine(LoadSceneAsyncCoroutine("Map", UIManager.Instance.loadingKeyBar));
    }

    IEnumerator LoadSceneAsyncCoroutine(string Map, Image loadingKeyBar)
    {
        yield return null;
        operation = SceneManager.LoadSceneAsync(Map);

        operation.allowSceneActivation = false;
        float timer = 0;

        while (!operation.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (operation.progress < 0.9f)
            {
                loadingKeyBar.fillAmount = Mathf.Lerp(loadingKeyBar.fillAmount, operation.progress, timer);
                if (loadingKeyBar.fillAmount >= operation.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                loadingKeyBar.fillAmount = Mathf.Lerp(loadingKeyBar.fillAmount, 1f, timer);
                if (loadingKeyBar.fillAmount == 1.0f)
                {
                    operation.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch(scene.name)
        {
            case "Map":
                UIManager.Instance.EnterBusMapMaskIn();
                break;
        }
    }
}
