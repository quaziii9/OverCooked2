using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IntroManager : Singleton<IntroManager>
{
    private bool isSpace;
    private bool isLoading = true;

    private Vector3 targetTransform = new Vector3(2f, -0.6f, -10.3f);
    private Quaternion targetRotation = Quaternion.Euler(new Vector3(0.326138616f, 12.5265751f, 0.704141259f));
    private Vector3 velocity = Vector3.zero;

    public CinemachineVirtualCamera vanCamera;
    public CinemachineVirtualCamera shutterCamera;

    void Awake()
    {
        UIManager.Instance.shutter.SetActive(false);
        UIManager.Instance.spaceToStart.SetActive(true);

        UIManager.Instance.ingamePlayerUI.SetActive(false);

        UIManager.Instance.buttonUI.SetActive(false);
        UIManager.Instance.settingUI.SetActive(false);

        UIManager.Instance.underBarStop.SetActive(false);
        UIManager.Instance.underBarCancle.SetActive(false);

        //UIManager.Instance.LoadingUI.SetActive(true);

        //Camera.main.transform.position = new Vector3(-1.17f, 0.8f, -8.05f);
        //Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0, 3f, 0f));

        StartCoroutine("IntroSetting");
    }

    void FixedUpdate()
    {
        if (!isSpace && !isLoading && Input.GetKeyDown(KeyCode.Space))
        {           
            StartSpace();
        }
    }

    public void StartSpace()
    {
        isSpace = true;
        SoundManager.Instance.StartPlay();

        UIManager.Instance.shutterAnim.SetTrigger("ShutterOn");

        shutterCamera.Priority = 9;

        //Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetTransform, ref velocity, 3f);
        //Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, targetRotation, 3f);

        //Camera.main.transform.position = new Vector3(-3.07999992f, -0.449999988f, -9.89999962f);
        //Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0.326138616f, 12.5265751f, 0.704141259f));

        StartCoroutine("ShutterOut");
        
        UIManager.Instance.spaceToStart.SetActive(false);

        UIManager.Instance.ingamePlayerUI.SetActive(true);

        UIManager.Instance.buttonUI.SetActive(true);
        // UIManager.Instance.UnderBarStop.SetActive(true);

        UIManager.Instance.loadingUI.SetActive(false);

    }

    IEnumerator ShutterOut()
    {
        yield return new WaitForSeconds(2f);

        UIManager.Instance.shutter.SetActive(false);
    }

    IEnumerator IntroSetting()
    {
        yield return new WaitForSeconds(18f);

        //Camera.main.transform.position = new Vector3(-1.17f, 0.8f, -8.05f);
        //Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0, 3f, 0f));

        UIManager.Instance.shutter.SetActive(true);  
        isLoading = false;
    }
}
