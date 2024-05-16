using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    [Header("Van")]
    public GameObject shutter;
    public GameObject buttonUI;
    public GameObject ingamePlayerUI;

    [Header("Intro")]
    public GameObject loadingUI;
    public GameObject spaceToStart;

    [Header("Option")]
    public GameObject settingUI;
    public GameObject stopUI;
    public GameObject optionBlackUI;

    [Header("UnderBar")]
    public GameObject underBarCancle;
    public GameObject underBarStop;

    [Header("Animator")]
    public Animator shutterAnim;

    [Header("MaskTransitionUI")]
    public GameObject broccoliMask; // 크기를 변경할 RectTransform
    public GameObject pineappleMask; // 크기를 변경할 RectTransform
    public RectTransform broccoliMaskRect;
    public RectTransform pineappleMaskRect;
    public Vector2 pineappleOutMaskRect = new Vector2(7300, 7300);
    public Vector2 broccoliOutMaskRect = new Vector2(4300, 4300);
    public float broccoliDuration = 0.3f; // 변화에 걸리는 시간
    public float pineappleDuration = 0.5f; // 변화에 걸리는 시간

    [Header("Battle")]
    public GameObject battleUI;

    [Header("ExitLobbyUI")]
    public GameObject exitLobbyUI;
    public GameObject exitLobbyBlackUI;

    [Header("LoadingKeyUI")]
    public GameObject loadingKeyUI;
    public Image loadingKeyBar;

    [Header("Resolution")]  
    public TextMeshProUGUI resolutionText;
    public GameObject fullScreenButton;
    public GameObject fullScreenCheck;
    private bool windowScreen = true;
    private bool settingWindowScreen = true;
    private int resolutionArrNum = 4;
    private int settingResolutionArrNum = 4;
    private string[] resolutionTextArr = new string[] { "1280 x 720", "1280 x 800", "1680 x 1050", "1920 x 1080", "1920 x 1200", "2560 x 1600", "3072 x 1920" };

    public bool maskInEnd;
    //private bool maskOutEnd;
    //private bool isExit = false;
    //private bool isSetting = false;

    public void Start()
    {
        resolutionText.text = resolutionTextArr[resolutionArrNum];
        fullScreenCheck.SetActive(windowScreen);
    }

    private void Update()
    {
        //if(!isSetting && !isExit && Input.GetKeyDown(KeyCode.Escape)) StopUIOn();
        //if (!isSetting && isExit && Input.GetKeyDown(KeyCode.Escape)) StopUIOff();
    }

    #region Intro UI

    public void SettingOn()
    {
        optionBlackUI.SetActive(true);
        settingUI.SetActive(true);
        //isSetting = true;
    }

    public void SeetingOff()
    {
        optionBlackUI.SetActive(false);
        settingUI.SetActive(false);
        //isSetting = false;
    }

    public void StopUIOn()
    {
        optionBlackUI.SetActive(true);
        stopUI.SetActive(true);
       // isExit = true;
    }

    public void StopUIOff()
    {
        optionBlackUI.SetActive(false);
        stopUI.SetActive(false);
        // isExit = false;
    }
    #endregion


    #region SoundSquares

    public void SetBGMSquares(float volumeBGM, GameObject[] BGMSquares)
    {
        int j = 0;
        for (float i = 0; i < 1; i += 0.1f)
        {
            if (i < volumeBGM) //켜있는거
            {
                BGMSquares[j].transform.GetChild(0).gameObject.SetActive(false);
                BGMSquares[j].transform.GetChild(1).gameObject.SetActive(true);
            }
            else //꺼있는거
            {
                BGMSquares[j].transform.GetChild(0).gameObject.SetActive(true);
                BGMSquares[j].transform.GetChild(1).gameObject.SetActive(false);
            }
            j++;
        }
    }

    public void SetEffectSquares(float volumeEffect, GameObject[] effectSquares)
    {
        int j = 0;
        for (float i = 0; i < 1; i += 0.1f)
        {
            if (i < volumeEffect)
            {
                effectSquares[j].transform.GetChild(0).gameObject.SetActive(false);
                effectSquares[j].transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                effectSquares[j].transform.GetChild(0).gameObject.SetActive(true);
                effectSquares[j].transform.GetChild(1).gameObject.SetActive(false);
            }
            j++;
        }
    }

    #endregion


    #region Resolution
    public void ResolutionRightButton()
    {
        resolutionArrNum = (resolutionArrNum + 1) % 7;
        resolutionText.text = resolutionTextArr[resolutionArrNum];
    }

    public void ResolutionLeftButton()
    {
        if (resolutionArrNum == 0) resolutionArrNum = 6;
        else
            resolutionArrNum = (resolutionArrNum - 1) % 7;

        resolutionText.text = resolutionTextArr[resolutionArrNum];
    }

    public void ResolutionChange()
    {
        settingResolutionArrNum = resolutionArrNum;
        settingWindowScreen = windowScreen;
        switch (resolutionArrNum)
        {
            case 0:
                Screen.SetResolution(1280, 720, !windowScreen);
                break;
            case 1:
                Screen.SetResolution(1280, 800, !windowScreen);
                break;
            case 2:
                Screen.SetResolution(1680, 1050, !windowScreen);
                break;
            case 3:
                Screen.SetResolution(1920, 1080, !windowScreen);
                break;
            case 4:
                Screen.SetResolution(1920, 1200, !windowScreen);
                break;
            case 5:
                Screen.SetResolution(2560, 1600, !windowScreen);
                break;
            case 6:
                Screen.SetResolution(3070, 1920, !windowScreen);
                break;
        }
    }

    public void CancleChange()
    {
        resolutionArrNum = settingResolutionArrNum;
        resolutionText.text = resolutionTextArr[resolutionArrNum];
        windowScreen = settingWindowScreen;
        fullScreenCheck.SetActive(windowScreen);
    }
    public void OnClickFullScreenButton()
    {
        windowScreen = !windowScreen;
        fullScreenCheck.SetActive(windowScreen);
    }
    #endregion



    #region Mask InOut UI

    public void MaskInUI(GameObject inMask, RectTransform inMaskRect, float Duration, string goTo)
    {
        maskInEnd = false;
        SoundManager.Instance.ScreenInUI();
        inMask.SetActive(true);
        inMaskRect = inMask.GetComponent<RectTransform>();
        StartCoroutine(MaskInOut(inMaskRect, Vector2.zero, Duration, () =>
        {
            maskInEnd = true;
            switch(goTo)
            {
                case "BattleUI":
                    Invoke("BattleUI", 1F);
                    break;
                case "BattleUIOff":
                    Invoke("BattleUIOff", 1F);
                    break;
                case "LoadingKeyUIOn":
                    Invoke("LoadingKeyUIOn", 1f);
                    break;
                case "EnterBusMapMaskOut":
                    Invoke("EnterBusMapMaskOut", 1f);
                    break;
            }        
        }));
    }

    public void MaskOutUI(GameObject inMask, GameObject outMask, RectTransform outMaskRect, Vector2 targetRect ,float Duration, string goTo)
    {
        SoundManager.Instance.ScreenOutUI();
        inMask.SetActive(false);
        outMask.SetActive(true);
        outMaskRect = outMask.GetComponent<RectTransform>();
        StartCoroutine(MaskInOut(outMaskRect, targetRect, Duration, () =>
        {
            //maskOutEnd = true;
            outMask.SetActive(false);

            switch (goTo)
            {
                case "GoToBusMap":
                    SceneChangeManager.Instance.ChangeToBusMap();
                    break;
                default:
                    break;
            }
        }));
    }

    IEnumerator MaskInOut(RectTransform rt, Vector2 toSize, float time, System.Action onComplete)
    {
        Vector2 fromSize = rt.sizeDelta;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            rt.sizeDelta = Vector2.Lerp(fromSize, toSize, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rt.sizeDelta = toSize; // 최종 크기 설정
        onComplete?.Invoke(); // 완료 시 콜백 호출
    }

    #endregion

    #region EnterBattleUI
    public void EnterBattleUI()
    {
        MaskInUI(broccoliMask, broccoliMaskRect, broccoliDuration, "BattleUI");
    }

    public void BattleUI()
    {
        battleUI.SetActive(true);
        MaskOutUI(broccoliMask, pineappleMask, pineappleMaskRect, pineappleOutMaskRect, pineappleDuration, "");
        SoundManager.Instance.FadeInAudio(SoundManager.Instance.bgmChangeAudioSource, 0, "Battle");
        SoundManager.Instance.FadeOutAudio(SoundManager.Instance.bgmAudioSource, 0);
    }

    public void BattleUIOff()
    {
        battleUI.SetActive(false);
        SoundManager.Instance.FadeInAudio(SoundManager.Instance.bgmAudioSource, 0, "Intro");
        SoundManager.Instance.FadeOutAudio(SoundManager.Instance.bgmChangeAudioSource, 0);
        MaskOutUI(pineappleMask, broccoliMask, broccoliMaskRect, broccoliOutMaskRect, broccoliDuration, "");
       
    }

    #endregion


    #region ExitBattleUI

    public void ExitLobbyUIOn()
    {
        exitLobbyBlackUI.SetActive(true);
        exitLobbyUI.SetActive(true);
    }

    public void CancleExitLobby()
    {
        exitLobbyBlackUI.SetActive(false);
        exitLobbyUI.SetActive(false);
    }

    public void ExitLobby()
    {
        exitLobbyBlackUI.SetActive(false);
        exitLobbyUI.SetActive(false);
        MaskInUI(pineappleMask, pineappleMaskRect, pineappleDuration, "BattleUIOff");
    }

    #endregion





    public void EnterLoadingKeyUI()
    {
        MaskInUI(broccoliMask, broccoliMaskRect, broccoliDuration, "LoadingKeyUIOn");
    }

    public void LoadingKeyUIOn()
    {
        loadingKeyUI.SetActive(true);
        MaskOutUI(broccoliMask, pineappleMask, pineappleMaskRect, pineappleOutMaskRect, pineappleDuration, "GoToBusMap");

    }

    public void EnterBusMapMaskIn()
    {
        MaskInUI(pineappleMask, pineappleMaskRect, pineappleDuration, "EnterBusMapMaskOut");
    }

    public void EnterBusMapMaskOut()
    {
        loadingKeyUI.SetActive(false);
        MaskOutUI(pineappleMask, broccoliMask, broccoliMaskRect, broccoliOutMaskRect, broccoliDuration, "");
    }


    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                            Application.Quit();
        #endif
        
    }
}
