using EnumTypes;
using UnityEngine;
using UnityEngine.UI;

public class StageUnlocked : MonoBehaviour
{
    public GameObject[] childObjects;
    public GameObject[] star;
    
    public int StageNum;
    public int StarNum;
    public Sprite YellowStar;
    public bool isLoading;

    public GameObject clickButton;   // 모바일 입장 버튼

    private void Start()
    {
        childObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects[i] = transform.GetChild(i).gameObject;
        }
        StarNum = MapManager.Instance.stages[StageNum];
        SetStar();
    }

    private void OnTriggerStay(Collider other)
    {
        // 해당 GameObject에 "Bus" 태그가 onTriggerEnter되면
        if (other.CompareTag("Bus"))
        {
            SetChildrenActive(true);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 해당 GameObject에 "Bus" 태그가 onTriggerExit되면
        if (other.CompareTag("Bus"))
        {
            // 자식 GameObject들을 비활성화
            SetChildrenActive(false);
            clickButton.SetActive(false);
        }
    }

    // 자식 GameObject들의 활성화 여부를 설정하는 함수
    private void SetChildrenActive(bool active)
    {
        switch (MapManager.Instance.stages[StageNum])
        {
            //스테이지매니저 내부의 자신의 번호와 동일한 스테이지의 값을조회
            case > 0:
            {
                foreach (GameObject childObject in childObjects) //자식들을 조회한뒤에 childObject중 unlock를 활성화
                {
                    if (childObject.name == "unlock")
                    {
                        childObject.SetActive(active);
                        clickButton.SetActive(true);
                        CheckSpace();
                    }
                }

                break;
            }
            case <= 0:
            {
                foreach (GameObject childObject in childObjects) //자식들을 조회한뒤에 childObject중 lock를 활성화
                {
                    if (childObject.name == "lock")
                    {
                        childObject.SetActive(active);
                    }
                }

                break;
            }
        }
    }

    private void CheckSpace()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || isLoading) return;
        
        isLoading = true;
        UIManager.Instance.mapType = MapType.Stage2_5;
        UIManager.Instance.sceneType = SceneType.StageMap;
        UIManager.Instance.EnterLoadingMapUI();
    }

    private void SetStar()
    {
        if (StarNum <= 0) return;
        {
            for (int i = 0; i < StarNum; i++)
            {
                Image starSprite = star[i].GetComponent<Image>();
                Debug.Log(i);
                Debug.Log(StarNum);
                starSprite.sprite = YellowStar;
            }
        }
    }
}