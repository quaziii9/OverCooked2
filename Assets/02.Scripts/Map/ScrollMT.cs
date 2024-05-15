using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMT : MonoBehaviour
{

    // 매쉬 랜더러와 마테리얼의 참조
    public MeshRenderer meshRenderer;
    public int materialIndex = 2; // 3번째 마테리얼의 인덱스 (0부터 시작)

    // 오프셋 변화에 대한 속성
    public float startYOffset = 0.25f; // 시작 Y 오프셋
    public float endYOffset = -0.23f; // 도달해야 할 Y 오프셋
    public float speed = 0.01f; // 오프셋 변화 속도

    private void Start()
    {
        meshRenderer= GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterials[materialIndex].mainTextureOffset = new Vector2(0f,0.25f);
    }
    void Update()
    {
        // 매쉬 랜더러와 마테리얼이 제대로 설정되었는지 확인
        if (meshRenderer == null || meshRenderer.sharedMaterials.Length <= materialIndex)
        {
            Debug.LogWarning("Mesh Renderer or Material not assigned properly.");
            return;
        }

        Vector2 currentOffset = meshRenderer.sharedMaterials[materialIndex].mainTextureOffset;

        if(currentOffset.y <= startYOffset)
        {
            currentOffset.y -= speed * Time.deltaTime;
        }

        if (currentOffset.y <= endYOffset)
        {
            currentOffset.y = startYOffset;
        }

        meshRenderer.sharedMaterials[materialIndex].mainTextureOffset = currentOffset;
    }
}
