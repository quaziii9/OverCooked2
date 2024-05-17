using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMT : MonoBehaviour
{

    // 매쉬 랜더러와 마테리얼의 참조
    public MeshRenderer meshRenderer;

    public int materialIndex = 0; // 3번째 마테리얼의 인덱스 (0부터 시작)
    // 오프셋 변화에 대한 속성



    public float speed = 0.5f; // 오프셋 변화 속도
    [Header("Y")]
    [SerializeField]
    public bool YPlus;
    [SerializeField]
    public bool y;
    [SerializeField]
    public bool yLimit;
    public float startYOffset = 0.0f; // 시작 Y 오프셋
    public float endYOffset = -0.0f; // 도달해야 할 Y 오프셋

    [Header("X")]
    [SerializeField]
    public bool XPlus;
    [SerializeField]
    public bool x;
    [SerializeField]
    public bool xLimit;
    public float startXOffset = 0.0f; // 시작 X 오프셋
    public float endXOffset = -0.0f; // 도달해야 할 X 오프셋

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterials[materialIndex].mainTextureOffset = new Vector2(0f, 0);
    }
    void Update()
    {
        // 매쉬 랜더러와 마테리얼이 제대로 설정되었는지 확인
        if (meshRenderer == null || meshRenderer.sharedMaterials.Length <= materialIndex)
        {
            Debug.LogWarning("Mesh Renderer or Material not assigned properly.");
            return;
        }

        //현재 오프셋
        Vector2 currentOffset = meshRenderer.sharedMaterials[materialIndex].mainTextureOffset;


        //y이동체크시
        if (y == true)
        {
            if (currentOffset.y <= startYOffset && YPlus)
            {
                currentOffset.y += speed * Time.deltaTime;
            }
            else if (currentOffset.y <= startYOffset && !YPlus)
            {
                currentOffset.y -= speed * Time.deltaTime;
            }
            else if(!yLimit&&!YPlus) //플러스가 아닐시
            {
                currentOffset.y -= speed * Time.deltaTime;
            }
            else if(!yLimit&&YPlus) // 플러스 일시
            {
                currentOffset.y += speed * Time.deltaTime;
            }

            //y제한 체크+리미트보다 작을시
            if (currentOffset.y <= endYOffset && yLimit && !YPlus)
            {
                currentOffset.y = startYOffset;
            }
            //y제한 체크+리미트보다 클시
            if (currentOffset.y >= endYOffset && yLimit && YPlus)
            {
                currentOffset.y = startYOffset;
            }
        }
        //x이동체크시
        if (x == true)
        {
            if (currentOffset.x <= startXOffset && XPlus)
            {
                currentOffset.x += speed * Time.deltaTime;
            }
            else if (currentOffset.x <= startXOffset && !XPlus)
            {
                currentOffset.x -= speed * Time.deltaTime;
            }
            else if (!xLimit && !XPlus) //플러스가 아닐시
            {
                currentOffset.x -= speed * Time.deltaTime;
            }
            else if (!xLimit && XPlus) // 플러스 일시
            {
                currentOffset.x += speed * Time.deltaTime;
            }

            //x제한 체크+리미트보다 작을시
            if (currentOffset.x <= endXOffset && xLimit && !XPlus)
            {
                currentOffset.x = startXOffset;
            }
            //x제한 체크+리미트보다 클시
            if (currentOffset.x >= endXOffset && xLimit && XPlus)
            {
                currentOffset.x = startXOffset;
            }
        }

        meshRenderer.sharedMaterials[materialIndex].mainTextureOffset = currentOffset;
    }
}
