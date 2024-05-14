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

    [Header("Sence Change UI")]
    public GameObject sceneChangeUI; // 크기를 변경할 RectTransform
    private RectTransform sceneChangeUIRect;
    public float duration = 0.3f; // 변화에 걸리는 시간



    //private bool isExit = false;
    //private bool isSetting = false;

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

    private void Update()
    {        
        //if(!isSetting && !isExit && Input.GetKeyDown(KeyCode.Escape)) StopUIOn();
        //if (!isSetting && isExit && Input.GetKeyDown(KeyCode.Escape)) StopUIOff();

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


    public void ExitGame()
    {

        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif

    }

    public void ShrinkToZero()
    {
        // 기존 코루틴을 중지하고 새로운 코루틴 시작
        //StopAllCoroutines();
        sceneChangeUI.SetActive(true);
        sceneChangeUIRect = sceneChangeUI.GetComponent<RectTransform>();
        StartCoroutine(ResizeRectTransform(sceneChangeUIRect, Vector2.zero, duration));
    }

    IEnumerator ResizeRectTransform(RectTransform rt, Vector2 toSize, float time)
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
}
