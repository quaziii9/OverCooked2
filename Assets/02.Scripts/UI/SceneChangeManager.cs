using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class SceneChangeManager : Singleton<SceneChangeManager>
{
    AsyncOperation operation;
    GameObject van;
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ChangeToBusMap()
    {
        SoundManager.Instance.FadeOutAudio(SoundManager.Instance.bgmAudioSource, 0);
        SoundManager.Instance.FadeInAudio(SoundManager.Instance.bgmChangeAudioSource, 0, "BusMap");
        StartCoroutine(LoadSceneAsyncCoroutine("Map", UIManager.Instance.loadingKeyBar));
    }

    public void ChangeToIntroMap()
    {
        SoundManager.Instance.FadeOutAudio(SoundManager.Instance.bgmChangeAudioSource, 0);
        SoundManager.Instance.FadeInAudio(SoundManager.Instance.bgmAudioSource, 0, "Intro");
        StartCoroutine(LoadSceneAsyncCoroutine("Intro", UIManager.Instance.loadingKeyBar));
    }

    public void ChangeToTestStage()
    {
        
        SoundManager.Instance.FadeOutAudio(SoundManager.Instance.bgmChangeAudioSource, 0);
        SoundManager.Instance.FadeInAudio(SoundManager.Instance.bgmAudioSource, 0, "StageMap");
        StartCoroutine(LoadSceneAsyncCoroutine("TestStage", UIManager.Instance.loadingMapBar));
    }

    IEnumerator LoadSceneAsyncCoroutine(string Map, Image loadingKeyBar)
    {
        yield return null;
        operation = SceneManager.LoadSceneAsync(Map);
        UIManager.Instance.LoadingFood();

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
                    UIManager.Instance.LoadingFoodOff();
                    yield break;
                }
            }
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Map":
                VanSingleton.Instance.van.SetActive(false);
                UIManager.Instance.EnterBusMapMaskIn();     
                
                break;
            case "Intro":
                UIManager.Instance.vanCamera = GameObject.Find("VanCam").GetComponent<CinemachineVirtualCamera>();
                UIManager.Instance.shutterCamera = GameObject.Find("ShutterCam").GetComponent<CinemachineVirtualCamera>();
                if (UIManager.Instance.first == false)
                {
                    VanSingleton.Instance.van.SetActive(true);
                    UIManager.Instance.busTopUI.SetActive(false);
                    UIManager.Instance.buttonUI.SetActive(true);
                    UIManager.Instance.shutterCamera.Priority = 9;
                    UIManager.Instance.EnterIntroMaskIn();
                }
                break;
            case "TestStage":
                VanSingleton.Instance.van.SetActive(false);
                UIManager.Instance.RecipeUIOn(0);
                UIManager.Instance.EnterTestStageMaskIn();
                break;
        }
    }

}
