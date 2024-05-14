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
    public GameObject sceneChangeUI;
    public float time;

    RectTransform sceneChangeUIRect;
    Vector2 fromSize;
    Vector2 toSize;

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

    public void SceneChangeUI()
    {
        sceneChangeUI.SetActive(true);
        sceneChangeUIRect = sceneChangeUI.GetComponent<RectTransform>();
        fromSize = sceneChangeUIRect.sizeDelta;
        toSize = new Vector2(0, 0);

        // 이 부분 수정해야함
    }

    IEnumerator SceneChaengeCo()
    {
        //Vector2 fromSize = rt.sizeDelta;

        sceneChangeUIRect.sizeDelta = Vector2.Lerp(fromSize, toSize, Time.deltaTime * 1f);

        yield return new WaitForSeconds(0.1f);
        //sceneChangeUIRect.sizeDelta = toSize; // 최종 크기 설정
    }
}
