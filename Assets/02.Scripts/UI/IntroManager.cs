using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    private bool isSpace;
    private bool isLoading = true;

    private const float INTRO_DURATION = 18f; // 인트로 지속 시간
    private const float SHUTTER_OUT_DELAY = 2f; // 셔터 비활성화 딜레이

    void Start()
    {
        InitUI();
    }

    void FixedUpdate()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        bool isInputReceived = Input.GetKeyDown(KeyCode.Space) ||
#if UNITY_ANDROID
                               (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
#else
                               false;
#endif

        if (!isSpace && !isLoading && isInputReceived)
        {
            StartSpace();
        }
    }

    // 처음 인트로 들어갈 때 세팅
    public void InitUI()
    {
        if (UIManager.Instance.first)
        {
            ActivateIntroUI();
            IntroSetting().Forget();
        }
    }

    private void ActivateIntroUI()
    {
        UIManager.Instance.loadingUI.SetActive(true);
        UIManager.Instance.spaceToStart.SetActive(true);
        UIManager.Instance.ingamePlayerUI.SetActive(false);
        UIManager.Instance.buttonUI.SetActive(false);
        UIManager.Instance.shutter.SetActive(true);
    }

    // 게임을 시작하려면 스페이스바
    public void StartSpace()
    {
        UIManager.Instance.first = false;
        isSpace = true;

        SoundManager.Instance.Load();
        SoundManager.Instance.StartPlay();

        ActivateGameUI();
        ShutterOut().Forget();

        UIManager.Instance.loadingUI.SetActive(false);
        UIManager.Instance.spaceToStart.SetActive(false);
    }

    private void ActivateGameUI()
    {
        UIManager.Instance.ingamePlayerUI.SetActive(true);
        UIManager.Instance.buttonUI.SetActive(true);
        UIManager.Instance.shutterAnim.SetTrigger("ShutterOn");
        SoundManager.Instance.VanShutter();
        UIManager.Instance.shutterCamera.Priority = 9;
    }

    private async UniTaskVoid ShutterOut()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(SHUTTER_OUT_DELAY));
        UIManager.Instance.shutter.SetActive(false);
    }

    private async UniTaskVoid IntroSetting()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(INTRO_DURATION));
        isLoading = false;
    }
}
