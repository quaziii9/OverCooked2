using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MobileResultSet : MonoBehaviour
{
    public TextMeshProUGUI recipeText;
    public GameObject recipeButtonUI;
    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            recipeText.text = "터치해서 계속하기";
            recipeButtonUI.SetActive(false);
        }
    }
}
