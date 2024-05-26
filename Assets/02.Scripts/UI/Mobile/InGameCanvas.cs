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
            coinUI_MobileBattle.SetActive(true);
            coinUI_PCBattle.SetActive(false);
        
    }
    
    private void MobilePlayerControllerSet()
    {
        mobilePlayerController.SetActive(true);
    }

}
