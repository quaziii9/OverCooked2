using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

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
    private RectTransform broccoliMaskRect;
    private RectTransform pineappleMaskRect;
    private Vector2 pineappleOutMaskRect = new Vector2(7300, 7300);
    private Vector2 broccoliOutMaskRect = new Vector2(4300, 4300);
    private float broccoliInDuration = 0.3f; // 변화에 걸리는 시간
    private float pineappleOutDuration = 0.5f; // 변화에 걸리는 시간

    [Header("Battle")]
    public GameObject battleUI;

    [Header("ExitLobbyUI")]
    public GameObject exitLobbyUI;
    public GameObject exitLobbyBlackUI;

    private bool maskInEnd;
    private bool maskOutEnd;
    //private bool isExit = false;
    //private bool isSetting = false;

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

 
    #region Mask InOut UI
   
    public void MaskInUI(GameObject inMask, RectTransform inMaskRect, float Duration)
    {
        SoundManager.Instance.ScreenInUI();
        inMask.SetActive(true);
        inMaskRect = inMask.GetComponent<RectTransform>();
        StartCoroutine(MaskInOut(inMaskRect, Vector2.zero, Duration, () => maskInEnd = true));
        maskInEnd = true;
    }

    public void MaskOutUI(GameObject inMask, GameObject outMask, RectTransform outMaskRect, Vector2 targetRect ,float Duration)
    {
        SoundManager.Instance.ScreenOutUI();
        inMask.SetActive(false);
        outMask.SetActive(true);
        outMaskRect = outMask.GetComponent<RectTransform>();
        StartCoroutine(MaskInOut(outMaskRect, targetRect, Duration, () =>
        {
            maskOutEnd = true;
            outMask.SetActive(false);
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
        MaskInUI(broccoliMask, broccoliMaskRect, broccoliInDuration);
        if(maskInEnd == true) Invoke("BattleUI", 1.5F);    
    }

    public void BattleUI()
    {
        battleUI.SetActive(true);
        MaskOutUI(broccoliMask, pineappleMask, pineappleMaskRect, pineappleOutMaskRect, pineappleOutDuration);
    }

    public void BattleUIOff()
    {
        battleUI.SetActive(false);
        MaskOutUI(pineappleMask, broccoliMask, broccoliMaskRect, broccoliOutMaskRect, broccoliInDuration);
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
        MaskInUI(pineappleMask, pineappleMaskRect, pineappleOutDuration);
        if (maskInEnd == true) Invoke("BattleUIOff", 1.5F);
        //battleUI.SetActive(false);
    }

    #endregion

    public void ExitGame()
    {

    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
                    Application.Quit();
    #endif

    }
}
