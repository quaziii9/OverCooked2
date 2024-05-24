using UnityEngine;
using UnityEngine.UI;

public class MobileSetOptionUI : MonoBehaviour
{

    public GameObject[] sliders; // 슬라이더를 담고 있는 배열
    public VerticalLayoutGroup verticalLayoutGroup;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AdjustForMobile();
        }
    }

    void AdjustForMobile()
    {
        // 필요한 슬라이더만 활성화
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].SetActive(false);
        }

        // Vertical Layout Group의 spacing 변경
        verticalLayoutGroup.spacing = 50;

        // 레이아웃 강제 업데이트
        LayoutRebuilder.ForceRebuildLayoutImmediate(verticalLayoutGroup.GetComponent<RectTransform>());
    }
}


