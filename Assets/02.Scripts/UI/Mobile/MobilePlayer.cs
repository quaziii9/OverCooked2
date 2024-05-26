using EnumTypes;
using EventLibrary;
using UnityEngine;

public class MobilePlayer : MonoBehaviour
{
    public GameObject mobilePlayerController;

    public void OnEnable()
    {
        EventManager<UIEvents>.StartListening(UIEvents.MobilePlayerController, MobilePlayerControllerSet);
    }
    
    private void MobilePlayerControllerSet()
    {
        mobilePlayerController.SetActive(true);
    }

}
