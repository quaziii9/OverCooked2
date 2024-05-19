using Cinemachine;
using System.Collections;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public CinemachineVirtualCamera vanCamera;
    public CinemachineVirtualCamera shutterCamera;

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
    private RectTransform broccoliMaskRect;
    private RectTransform pineappleMaskRect;
    private Vector2 pineappleOutMaskRect = new Vector2(7300, 7300);
    private Vector2 broccoliOutMaskRect = new Vector2(4300, 4300);
    private float broccoliDuration = 0.3f; // 변화에 걸리는 시간
    private float pineappleDuration = 0.5f; // 변화에 걸리는 시간

    [Header("Battle")]
    public GameObject battleUI;
    public GameObject battleResultUI;

    [Header("ExitLobbyUI")]
    public GameObject exitLobbyUI;
    public GameObject exitLobbyBlackUI;

    [Header("Loading")]
    public GameObject loadingKeyUI;
    public GameObject[] loadingFoodArr;
    public Image loadingKeyBar;
    public GameObject loadingMapUI;
    public Image loadingMapBar;

    [Header("BusMap")]
    public GameObject busTopUI;
    public GameObject busMapEscUI;
    public GameObject busMapEscBlackUI;

    [Header("StageMap")]
    public GameObject stageMapEscUI;
    public GameObject stageMapEscBlackUI;


    [Header("RecipeUI")]
    public GameObject recipeUI;
    public GameObject recipeBlackUI;
    public GameObject [] recipeArr;

    [Header("Resolution")]  
    public TextMeshProUGUI resolutionText;
    public GameObject fullScreenButton;
    public GameObject fullScreenCheck;
    public bool windowScreen;
    private bool settingWindowScreen;
    public int resolutionArrNum;
    private int settingResolutionArrNum;
    public string[] resolutionTextArr 
        = new string[] { "1280 x 720", "1680 x 1050", "1920 x 1080", "2560 x 1440", "3840 x 2160" };


    public bool first = true;
    //private bool maskInEnd;
    //private bool maskOutEnd;
    //private bool isExit = false;
    //private bool isSetting = false;


    public void Load()
    {
        settingWindowScreen = LoadData.Instance.optionData.saveWindowMode;
        windowScreen = LoadData.Instance.optionData.saveWindowMode;
        resolutionArrNum = LoadData.Instance.optionData.saveResolutionNum;
        settingResolutionArrNum = LoadData.Instance.optionData.saveResolutionNum;
        SetResolution();
        resolutionText.text = resolutionTextArr[resolutionArrNum];
        fullScreenCheck.SetActive(windowScreen);
    }


    public void Start()
    {
        resolutionText.text = resolutionTextArr[resolutionArrNum];
        fullScreenCheck.SetActive(windowScreen);
        broccoliMaskRect = broccoliMask.GetComponent<RectTransform>();
        pineappleMaskRect = pineappleMask.GetComponent<RectTransform>();
    }

    private void Update()
    {
        EscUI();
        StageEscUI();
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
       
    }

    public void StopUIOff()
    {
        optionBlackUI.SetActive(false);
        stopUI.SetActive(false);

        if(SceneManager.GetActiveScene().name == "Map")
        {
            busMapEscBlackUI.SetActive(false);
            busMapEscUI.SetActive(false);
        }

        stageMapEscBlackUI.SetActive(false);
        stageMapEscUI.SetActive(false);
        
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
        resolutionArrNum = (resolutionArrNum + 1) % 5;
        resolutionText.text = resolutionTextArr[resolutionArrNum];
    }

    public void ResolutionLeftButton()
    {
        if (resolutionArrNum == 0) resolutionArrNum = 4;
        else
            resolutionArrNum = (resolutionArrNum - 1) % 5;

        resolutionText.text = resolutionTextArr[resolutionArrNum];
    }

    public void ResolutionChange()
    {
        settingResolutionArrNum = resolutionArrNum;
        settingWindowScreen = windowScreen;

        LoadData.Instance.SaveOptionDataToJson();

        SetResolution();
    }

    public void SetResolution()
    {
        switch (resolutionArrNum)
        {
            case 0:
                Screen.SetResolution(1280, 720, !windowScreen);
                break;
            case 1:
                Screen.SetResolution(1680, 1050, !windowScreen);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, !windowScreen);
                break;
            case 3:
                Screen.SetResolution(2560, 1440, !windowScreen);
                break;
            case 4:
                Screen.SetResolution(3840, 2160, !windowScreen);
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
        //maskInEnd = false;
        SoundManager.Instance.ScreenInUI();
        inMask.SetActive(true);
        inMaskRect = inMask.GetComponent<RectTransform>();
        StartCoroutine(MaskInOut(inMaskRect, Vector2.zero, Duration, () =>
        {
            LoadingFood();

            // maskInEnd = true;
            switch (goTo)
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
                case "LoadingKeyUIToIntro":
                    busTopUI.SetActive(false);
                    Invoke("LoadingKeyUIToIntro", 1f);
                    break;
                case "LoadingMapUIOn":                   
                    Invoke("LoadingMapUIOn", 1f);
                    break;
                case "EnterIntroMapMaskOut":
                    Invoke("EnterIntroMapMaskOut", 1f);
                    break;
                case "EnterTestStageMaskIn":
                    Invoke("EnterTestStageMaskOut", 1f);
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
        if (goTo == "") Invoke("LoadingFoodOff", 0.5f);
        else LoadingFoodOff();
        StartCoroutine(MaskInOut(outMaskRect, targetRect, Duration, () =>
        {
            //maskOutEnd = true;
            outMask.SetActive(false);

            switch (goTo)
            {
                case "GoToBusMap":
                    SceneChangeManager.Instance.ChangeToBusMap();
                    loadingKeyBar.fillAmount = 0;
                    break;
                case "GoToIntroMap":
                    SceneChangeManager.Instance.ChangeToIntroMap();
                    loadingKeyBar.fillAmount = 0;
                    break;
                case "busTopUI":
                    busTopUI.SetActive(true);
                    break;
                case "GoToTestStage":
                    SceneChangeManager.Instance.ChangeToTestStage();
                    loadingMapBar.fillAmount = 0;
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


    public void EnterLoadingMapUI()
    {
        MaskInUI(pineappleMask, pineappleMaskRect, pineappleDuration, "LoadingMapUIOn");
    }

    public void LoadingMapUIOn()
    {
        loadingMapUI.SetActive(true);
        battleUI.SetActive(false);
        MaskOutUI(pineappleMask, broccoliMask, broccoliMaskRect, broccoliOutMaskRect, broccoliDuration, "GoToTestStage");
    }


    public void EnterBusMapMaskIn()
    {
        MaskInUI(pineappleMask, pineappleMaskRect, pineappleDuration, "EnterBusMapMaskOut");
    }

    public void EnterIntroMapMaskIn()
    {
        MaskInUI(pineappleMask, pineappleMaskRect, pineappleDuration, "EnterIntroMapMaskOut");
    }

    public void EnterTestStageMaskIn()
    {
        MaskInUI(broccoliMask, broccoliMaskRect, broccoliDuration, "EnterTestStageMaskIn");
    }

    public void EnterTestStageMaskOut()
    {
        loadingMapUI.SetActive(false);
        MaskOutUI(broccoliMask, pineappleMask, pineappleMaskRect, pineappleOutMaskRect, pineappleDuration, "");
    }


    public void EnterBusMapMaskOut()
    {
        loadingKeyUI.SetActive(false);
        MaskOutUI(pineappleMask, broccoliMask, broccoliMaskRect, broccoliOutMaskRect, broccoliDuration, "busTopUI");
    }

    public void EnterIntroMapMaskOut()
    {
        loadingKeyUI.SetActive(false);
        MaskOutUI(pineappleMask, broccoliMask, broccoliMaskRect, broccoliOutMaskRect, broccoliDuration, "");
    }

  
    public void EscUI()
    {
        if(SceneManager.GetActiveScene().name == "Map")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                busMapEscBlackUI.SetActive(true);
                busMapEscUI.SetActive(true);
                SoundManager.Instance.ButtonTick();
            }
        }
    }

    public void StageEscUI()
    {
        if (SceneManager.GetActiveScene().name == "TestStage")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                stageMapEscBlackUI.SetActive(true);
                stageMapEscUI.SetActive(true);
                SoundManager.Instance.ButtonTick();
            }
        }
    }
    public void StageEscUICancle()
    {
        stageMapEscBlackUI.SetActive(false);
        stageMapEscUI.SetActive(false);
    }


    public void EscUICancle()
    {
        busMapEscBlackUI.SetActive(false);
        busMapEscUI.SetActive(false);
    }

    public void EscUIStop()
    {
        optionBlackUI.SetActive(true);
        stopUI.SetActive(true);
    }

    public void EnterIntro()
    {
        broccoliMaskRect.sizeDelta = new Vector2(4300, 4300);
        MaskInUI(broccoliMask, broccoliMaskRect, broccoliDuration, "LoadingKeyUIToIntro");
    }

    public void LoadingKeyUIToIntro()
    {
        loadingKeyUI.SetActive(true);
        pineappleMaskRect.sizeDelta = Vector2.zero;
        MaskOutUI(broccoliMask, pineappleMask, pineappleMaskRect, pineappleOutMaskRect, pineappleDuration, "GoToIntroMap");
    }

    public void LoadingFood()
    {
        switch(Random.Range(0, 2))
        {
            case 0:
                loadingFoodArr[0].SetActive(true);
                loadingFoodArr[1].SetActive(false);
                loadingFoodArr[2].SetActive(false);
                break;
            case 1:
                loadingFoodArr[0].SetActive(false);
                loadingFoodArr[1].SetActive(true);
                loadingFoodArr[2].SetActive(false);
                break;
            case 2:
                loadingFoodArr[0].SetActive(false);
                loadingFoodArr[1].SetActive(false);
                loadingFoodArr[2].SetActive(true);
                break;
        }
    }

    public void LoadingFoodOff()
    {
        loadingFoodArr[0].SetActive(false);
        loadingFoodArr[1].SetActive(false);
        loadingFoodArr[2].SetActive(false);
    }

    public void RecipeUIOn(int arr)
    {
        recipeBlackUI.SetActive(true);
        recipeUI.SetActive(true);
        recipeArr[arr].SetActive(true);   
    }

    public void RecipeUIOff(int arr)
    {
        recipeBlackUI.SetActive(false);
        recipeUI.SetActive(false);
        recipeArr[arr].SetActive(false);
        
    }

    public void ExitGame()
    {
        if (SceneManager.GetActiveScene().name == "Map")
        {
            optionBlackUI.SetActive(false);
            stopUI.SetActive(false);
            busMapEscBlackUI.SetActive(false);
            busMapEscUI.SetActive(false);
            EnterIntro();
        }
        else if (SceneManager.GetActiveScene().name == "TestStage")
        {
            optionBlackUI.SetActive(false);
            stopUI.SetActive(false);
            stageMapEscBlackUI.SetActive(false);
            stageMapEscUI.SetActive(false);
            EnterIntro();
        }

        else
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                                                    Application.Quit();
            #endif
        }
    }
}