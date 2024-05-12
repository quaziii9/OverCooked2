using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : Singleton<IntroManager>
{
    private bool isSpace;
    private bool isLoading = true;

  
    void Awake()
    {
        UIManager.Instance.Shutter.SetActive(false);
        UIManager.Instance.SpaceToStart.SetActive(true);

        UIManager.Instance.IngamePlayerUI.SetActive(false);

        UIManager.Instance.ButtonUI.SetActive(false);
        UIManager.Instance.SettingUI.SetActive(false);

        UIManager.Instance.UnderBarStop.SetActive(false);
        UIManager.Instance.UnderBarCancle.SetActive(false);

        //UIManager.Instance.LoadingUI.SetActive(true);

        StartCoroutine("IntroSetting");
    }

    void Update()
    {
        if (!isSpace && !isLoading && Input.GetKeyDown(KeyCode.Space))
        {
            isSpace = true;

            Camera.main.transform.position = new Vector3(-3.07999992f, -0.449999988f, -9.89999962f);
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0.326138616f, 12.5265751f, 0.704141259f));

            UIManager.Instance.Shutter.SetActive(false);
            UIManager.Instance.SpaceToStart.SetActive(false);

            UIManager.Instance.IngamePlayerUI.SetActive(true);
            
            UIManager.Instance.ButtonUI.SetActive(true);
           // UIManager.Instance.UnderBarStop.SetActive(true);

            UIManager.Instance.LoadingUI.SetActive(false);

        }
    }

    IEnumerator IntroSetting()
    {
        yield return new WaitForSeconds(18f);

        Camera.main.transform.position = new Vector3(-1.83f, 0.58f, -7.84f);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(359.932678f, 7.07423258f, 0.24220185f));

        UIManager.Instance.Shutter.SetActive(true);  
        isLoading = false;
    }
}
