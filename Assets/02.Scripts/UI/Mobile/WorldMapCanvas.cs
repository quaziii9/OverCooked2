using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapCanvas : MonoBehaviour
{
    public GameObject mobileBusController;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            mobileBusController.SetActive(true);
        }
        else
            mobileBusController.SetActive(false);

    }
}
