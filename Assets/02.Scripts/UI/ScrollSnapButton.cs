using DanielLochner.Assets.SimpleScrollSnap;
using Org.BouncyCastle.Bcpg.OpenPgp;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollSnapButton : MonoBehaviour
{
    public SimpleScrollSnap snap;
    public Sprite selectImg;
    public Sprite notselectImg;
    public Image backImage;
    public GameObject MapObject;
    
    string name;

    public void Start()
    {
        snap.GetComponent<SimpleScrollSnap>();
    }

    public void Update()
    {
        if (snap.OnPanelSelectedGetName() == MapObject.name)
        {
            backImage.sprite = selectImg;
        }
        else
        {
            backImage.sprite = notselectImg;
        }
    }

    public void SnapMapClick()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        name = clickObject.name;
        if (snap.OnPanelSelectedGetName() == name)
        {
            UIManager.Instance.EnterLoadingMapUI();
        }
    }
}
