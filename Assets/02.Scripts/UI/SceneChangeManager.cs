using Cinemachine;
using EnumTypes;
using EventLibrary;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager : Singleton<SceneChangeManager>
{
    AsyncOperation operation;
    private void OnEnable()
    {
        // 씬 로드 완료 이벤트에 OnSceneLoaded 메서드를 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventManager<UIEvents>.StartListening(UIEvents.WorldMapOpen, ChangeToBusMap);
        EventManager<UIEvents>.StartListening(UIEvents.IntroMapOpen, ChangeToIntroMap);
        EventManager<UIEvents>.StartListening(UIEvents.TestStageMapOpen, ChangeToTestStage);
        EventManager<UIEvents>.StartListening(UIEvents.BattleRoomOpen, ChangeToBattleLobby);
    }

    private void OnDisable()
    {
        // 씬 로드 완료 이벤트에서 OnSceneLoaded 메서드를 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 버스 맵으로 전환
    public void ChangeToBusMap()
    {
        ChangeScene("BusMap", "Map", UIManager.Instance.loadingKeyBar);
    }

    // 인트로 맵으로 전환
    public void ChangeToIntroMap()
    {
        ChangeScene("Intro", "Intro", UIManager.Instance.loadingKeyBar);
    }

    // 테스트 스테이지로 전환
    public void ChangeToTestStage()
    {
        ChangeScene("StageMap", "TestStage", UIManager.Instance.loadingMapBar);
    }

    public void ChangeToBattleLobby()
    {
        ChangeScene("Battle", "BattleLobby", UIManager.Instance.loadingKeyBar);
    }

    // 씬 전환을 위한 공통 메서드
    private void ChangeScene(string bgmName, string sceneName, Image loadingBar)
    {
        if(SoundManager.Instance.bgmChangeAudioSource.isPlaying)
        {
            SoundManager.Instance.FadeOutAudio(SoundManager.Instance.bgmChangeAudioSource, 0);
            SoundManager.Instance.FadeInAudio(SoundManager.Instance.bgmAudioSource, 0, bgmName);
        }
        else if(SoundManager.Instance.bgmAudioSource.isPlaying)
        {
            SoundManager.Instance.FadeOutAudio(SoundManager.Instance.bgmAudioSource, 0);
            SoundManager.Instance.FadeInAudio(SoundManager.Instance.bgmChangeAudioSource, 0, bgmName);
        }
       

        StartCoroutine(LoadSceneAsyncCoroutine(sceneName, loadingBar));
    }

    // 씬을 비동기로 로드하는 코루틴
    IEnumerator LoadSceneAsyncCoroutine(string sceneName, Image loadingBar)
    {
        yield return null;
        operation = SceneManager.LoadSceneAsync(sceneName);
        UIManager.Instance.LoadingFood();

        operation.allowSceneActivation = false;
        float timer = 0;

        while (!operation.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (operation.progress < 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, operation.progress, timer);
                if (loadingBar.fillAmount >= operation.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, timer);
                if (loadingBar.fillAmount == 1.0f)
                {
                    operation.allowSceneActivation = true;
                    UIManager.Instance.LoadingFoodOff();
                    yield break;
                }
            }
        }
    }

    // 씬 로드 완료 후 호출되는 메서드
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 로드 완료: {scene.name}");
        switch (scene.name)
        {
            case "Map":
                VanSingleton.Instance.van.SetActive(false);
                UIManager.Instance.EnterBusMapMaskIn();
                UIManager.Instance.sceneType = SceneType.BusMap;
                break;
            case "Intro":
                UIManager.Instance.vanCamera = GameObject.Find("VanCam").GetComponent<CinemachineVirtualCamera>();
                UIManager.Instance.shutterCamera = GameObject.Find("ShutterCam").GetComponent<CinemachineVirtualCamera>();
                UIManager.Instance.sceneType = SceneType.Intro;
                if (!UIManager.Instance.first)
                {
                    VanSingleton.Instance.van.SetActive(true);
                    UIManager.Instance.busTopUI.SetActive(false);
                    UIManager.Instance.buttonUI.SetActive(true);
                    UIManager.Instance.exitLobbyUI.SetActive(false);
                    UIManager.Instance.battleUI.SetActive(false);
                    UIManager.Instance.shutterCamera.Priority = 9;
                    UIManager.Instance.EnterIntroMaskIn();
                }
                break;
            case "TestStage":
                UIManager.Instance.sceneType = SceneType.BattleMap;
                VanSingleton.Instance.van.SetActive(false);
                UIManager.Instance.RecipeUIOn(0);
                UIManager.Instance.EnterTestStageMaskIn();
                break;
            case "BattleLobby":
                UIManager.Instance.sceneType = SceneType.BattleLobby;
                VanSingleton.Instance.van.SetActive(false);
                UIManager.Instance.buttonUI.SetActive(false);
                UIManager.Instance.battleUI.SetActive(true);
                UIManager.Instance.BattleUIOn();
                break;
        }
    }
}
