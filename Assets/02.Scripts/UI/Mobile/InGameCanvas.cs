using EnumTypes;
using EventLibrary;
using JetBrains.Annotations;
using UnityEngine;

public class InGameCanvas : MonoBehaviour
{
    public GameObject mobilePlayerController;
    public GameObject coinUI_PCBattle;
    public GameObject coinUI_MobileBattle;
    //public GameObject coinUI_Single;

    public void OnEnable()
    {
        EventManager<UIEvents>.StartListening(UIEvents.MobilePlayerController, MobilePlayerControllerSet);
    }

    public void Start()
    {
        SetCointUI();
    }

    public void SetCointUI()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            coinUI_MobileBattle.SetActive(true);
            coinUI_PCBattle.SetActive(false);
        }
        else
        {
            coinUI_MobileBattle.SetActive(false);
            coinUI_PCBattle.SetActive(true);
        }
    }
    
    private void MobilePlayerControllerSet()
    {
        mobilePlayerController.SetActive(true);
    }

}
