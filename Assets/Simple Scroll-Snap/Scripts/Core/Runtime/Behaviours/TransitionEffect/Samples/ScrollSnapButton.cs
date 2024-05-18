using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollSnapButton : MonoBehaviour
{
    public SimpleScrollSnap snap;

    string name;

    public void Start()
    {
        snap.GetComponent<SimpleScrollSnap>();
    }

    public void SnapMapClick()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        name = clickObject.name;
        if (snap.OnPanelSelectedGetName() == name)
        {
            Debug.Log("성공 : " +name);
        }
    }
}
