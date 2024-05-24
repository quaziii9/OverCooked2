using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image backgroundImage;
    public Sprite highlightSprite;
    public Sprite normalSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Application.platform != RuntimePlatform.Android)
        {
            backgroundImage.sprite = highlightSprite;
        }
        // 마우스 또는 터치가 버튼 위로 올라갔을 때 하이라이트 이미지를 설정합니다.
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 마우스 또는 터치가 버튼에서 나갔을 때 원래 이미지를 설정합니다.
        if (Application.platform != RuntimePlatform.Android)
        {
            backgroundImage.sprite = normalSprite;
        }
    }
}
