using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSettingManager : MonoBehaviour
{
    [SerializeField] GameObject SettingUI;

    public void SettingOnButton()
    {
        SettingUI.SetActive(true);
    }

    public void SeetingOffButton()
    {
        SettingUI.SetActive(false);
    }
}
