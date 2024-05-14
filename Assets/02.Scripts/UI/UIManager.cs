using System.Collections;
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
    public GameObject popupBackGroundUI;

    [Header("UnderBar")]
    public GameObject underBarCancle;
    public GameObject underBarStop;

    [Header("Animator")]
    public Animator shutterAnim;

    [Header("MaskTransitionUI")]
    public GameObject maskInUI; // 크기를 변경할 RectTransform
    public GameObject maskOutUI; // 크기를 변경할 RectTransform
    private RectTransform maskInUIRect;
    private RectTransform maskOutUIRect;
    private Vector2 maskOutTargetRect = new Vector2(7300, 7300);
    public float inDuration = 0.3f; // 변화에 걸리는 시간
    public float outDuration = 0.5f; // 변화에 걸리는 시간

    [Header("Battle")]
    public GameObject battleUI;

    [Header("ExitLobbyUI")]
    public GameObject exitLobbyUI;

    public bool maskInEnd;
    public bool maskOutEnd;
    public bool battleIn;
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
        popupBackGroundUI.SetActive(true);
        settingUI.SetActive(true);
        //isSetting = true;
    }

    public void SeetingOff()
    {
        popupBackGroundUI.SetActive(false);
        settingUI.SetActive(false);
        //isSetting = false;
    }

    public void StopUIOn()
    {
        popupBackGroundUI.SetActive(true);
        stopUI.SetActive(true);
       // isExit = true;
    }

    public void StopUIOff()
    {
        popupBackGroundUI.SetActive(false);
        stopUI.SetActive(false);
        // isExit = false;
    }
    #endregion


    #region

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

    public void ExitGame()
    {

        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif

    }




    // PopUpIn -> UI팝업 띄우기 -> PopUpOut 
    #region Scene Change UI
   
    public void MaskInUI()
    {
        maskInUI.SetActive(true);
        maskInUIRect = maskInUI.GetComponent<RectTransform>();
        StartCoroutine(MaskInOut(maskInUIRect, Vector2.zero, inDuration));
        maskInEnd = true;
    }

    public void MaskOutUI()
    {
        maskInUI.SetActive(false);
        maskOutUI.SetActive(true);
        maskOutUIRect = maskOutUI.GetComponent<RectTransform>();
        StartCoroutine(MaskInOut(maskOutUIRect, maskOutTargetRect, outDuration));

        maskOutEnd = true;
    }

    IEnumerator MaskInOut(RectTransform rt, Vector2 toSize, float time)
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
    }

    #endregion

    public void EnterBattleUI()
    {
        MaskInUI();
        if(maskInEnd == true) Invoke("BattleUI", 2F);

        Debug.Log(maskOutEnd);
        if (maskOutEnd == true)
        {
            Debug.Log("??");
            maskOutUI.SetActive(false);
        }
        Debug.Log(maskOutEnd);
        Debug.Log("?");
    }

    public void BattleUI()
    {
        battleUI.SetActive(true);
        MaskOutUI();
        maskOutEnd = true;
    }

    public void ExitLobbyUIOn()
    {
        popupBackGroundUI.SetActive(true);
        exitLobbyUI.SetActive(true);
    }

    public void ExitLobbyUIOff()
    {
        popupBackGroundUI.SetActive(false);
        exitLobbyUI.SetActive(false);
    }

    public void Enter()
    {
        battleUI.SetActive(false);
    }
}
