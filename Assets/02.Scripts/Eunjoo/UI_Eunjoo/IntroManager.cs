using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField] GameObject Shutter;
    [SerializeField] GameObject SpaceToStart;
    [SerializeField] GameObject ButtonUI;
    [SerializeField] GameObject SettingUI;
    [SerializeField] GameObject IngamePlayerUI;
    [SerializeField] GameObject LoadingUI;

    private bool isSpace;
    private bool isLoading = true;

  
    void Awake()
    {
        Shutter.SetActive(false);
        SpaceToStart.SetActive(true);
        IngamePlayerUI.SetActive(false);
        ButtonUI.SetActive(false);
        SettingUI.SetActive(false);
        StartCoroutine("IntroSetting");
    }

    void Update()
    {
        if (!isSpace && !isLoading && Input.GetKeyDown(KeyCode.Space))
        {
            isSpace = true;
            Camera.main.transform.position = new Vector3(-3.07999992f, -0.449999988f, -9.89999962f);
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0.326138616f, 12.5265751f, 0.704141259f));
            Shutter.SetActive(false);
            SpaceToStart.SetActive(false);
            IngamePlayerUI.SetActive(true);
            ButtonUI.SetActive(true);
        }
    }

    IEnumerator IntroSetting()
    {
        yield return new WaitForSeconds(18f);

        Camera.main.transform.position = new Vector3(-1.83f, 0.58f, -7.84f);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(359.932678f, 7.07423258f, 0.24220185f));
        Shutter.SetActive(true);
        SpaceToStart.SetActive(true);
        ButtonUI.SetActive(false);
        IngamePlayerUI.SetActive(false);
        LoadingUI.SetActive(false);
        isLoading = false;
    }

   
    
}
