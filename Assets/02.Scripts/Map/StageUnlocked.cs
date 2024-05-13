using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageUnlocked : MonoBehaviour
{
    public float scaleSpeed = 0.5f;
    public float minScale = 0.2f;
    public float midScale = 0.5f;
    public float maxScale = 1.0f;   
    public GameObject[] childObjects;
    public GameObject[] star;
    //[SerializeField]
    public int StageNum;
    public int StarNum;
    public Sprite YellowStar;

    void Start()
    {
        childObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects[i] = transform.GetChild(i).gameObject;
        }
        StarNum = StageManager.Instance.stages[StageNum];
        SetStar();
        
    }
    private void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        // 해당 GameObject에 "Bus" 태그가 onenter되면
        if (other.CompareTag("Bus"))
        {
            SetChildrenActive(true);
            //foreach (GameObject childObject in childObjects)
            //{
            //    //childObject.transform.localScale = Vector3.one;
            //    childObject.transform.localScale = new Vector3 (midScale, midScale, midScale);
            //}
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 해당 GameObject에 "Bus" 태그가 exit되면
        if (other.CompareTag("Bus"))
        {
            // 스케일을 중간 크기로 설정
            //foreach (GameObject childObject in childObjects)
            //{
            //    childObject.transform.localScale = Vector3.one * minScale;
            //}

            // 자식 GameObject들을 비활성화
            SetChildrenActive(false);
        }
    }

    // 자식 GameObject들의 활성화 여부를 설정하는 함수
    void SetChildrenActive(bool active)
    {
        if (StageManager.Instance.stages[StageNum] >= 0)//스테이지매니저 내부의 자신의 번호와 동일한 스테이지의 값을조회
        {
            foreach (GameObject childObject in childObjects) //자식들을 조회한뒤에 childObject중 unlock를 활성화
            {
                if (childObject.name == "unlock")
                {
                    childObject.SetActive(active);

                    //foreach (Transform child in childObject.transform)
                    //{
                    //    if (child.name == "square")
                    //    {
                    //        foreach (Transform squareChild in child)
                    //        {
                    //            if (squareChild.name == "unlock")
                    //            {
                    //                GameObject[] star = new GameObject[squareChild.transform.childCount];
                    //                for (int i = 0; i < squareChild.transform.childCount; i++)
                    //                {
                    //                    Transform ob = squareChild.transform.GetChild(i);

                    //                    star[i] = ob.gameObject;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}


                }
            }
        }
        else if (StageManager.Instance.stages[StageNum] <= 0)
        {
            foreach (GameObject childObject in childObjects) //자식들을 조회한뒤에 childObject중 lock를 활성화
            {
                if (childObject.name == "lock")
                {
                    childObject.SetActive(active);
                }
            }
        }
    }

    void SetStar()
    {
        if (StarNum > 0)
        {
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

    IEnumerator ScaleDown()
    {
        while (childObjects[0].transform.localScale.magnitude > minScale)
        {
            // StageUI 태그를 가진 자식 GameObject들의 스케일을 축소
            foreach (GameObject stageUIObject in childObjects)
            {
                stageUIObject.transform.localScale -= Vector3.one * scaleSpeed * Time.deltaTime;
            }

            yield return null;
        }
    }

    IEnumerator ScaleUp()
    {
        // 최대 크기에 도달할 때까지 반복
        while (childObjects[0].transform.localScale.magnitude < maxScale)
        {
            // StageUI 태그를 가진 자식 GameObject들의 스케일을 확대
            foreach (GameObject stageUIObject in childObjects)
            {
                stageUIObject.transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
            }

            yield return null;
        }
    }



}