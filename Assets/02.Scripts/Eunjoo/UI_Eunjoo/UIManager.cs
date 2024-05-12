using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    [Header("Van")]
    public GameObject Shutter;
    public GameObject ButtonUI;
    public GameObject IngamePlayerUI;

    [Header("Intro")]
    public GameObject LoadingUI;
    public GameObject SpaceToStart;

    [Header("Option")]
    public GameObject SettingUI;
    public GameObject StopUI;
    public GameObject PopupBackGroundUI;

    [Header("UnderBar")]
    public GameObject UnderBarCancle;
    public GameObject UnderBarStop;


    //private bool isExit = false;
    //private bool isSetting = false;

    public void SettingOn()
    {
        PopupBackGroundUI.SetActive(true);
        SettingUI.SetActive(true);
        //isSetting = true;
    }

    public void SeetingOff()
    {
        PopupBackGroundUI.SetActive(false);
        SettingUI.SetActive(false);
        //isSetting = false;
    }

    private void Update()
    {        
        //if(!isSetting && !isExit && Input.GetKeyDown(KeyCode.Escape)) StopUIOn();
        //if (!isSetting && isExit && Input.GetKeyDown(KeyCode.Escape)) StopUIOff();

    }


    public void StopUIOn()
    {
        PopupBackGroundUI.SetActive(true);
        StopUI.SetActive(true);
       // isExit = true;
    }

    public void StopUIOff()
    {
        PopupBackGroundUI.SetActive(false);
        StopUI.SetActive(false);
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

}
