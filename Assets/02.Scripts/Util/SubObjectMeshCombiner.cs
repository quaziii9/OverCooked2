using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubMeshCombiner : MonoBehaviour
{
    public GameObject parent; // 메쉬를 병합할 부모 오브젝트
    public bool addMeshCollider = false; // 병합된 메쉬에 메쉬 콜라이더를 추가할지 결정하는 변수
    public bool deactivateParentAfterMerge = true; // 병합 후 부모 오브젝트를 비활성화할지 결정하는 변수
    public bool destroyParentAfterMerge = false; // 병합 후 부모 오브젝트를 삭제할지 결정하는 변수

    [ContextMenu("Merge")]
    public void MergeMeshes()
    {
        // 부모 오브젝트가 설정되지 않았다면 에러 메시지를 출력하고 메서드를 종료합니다.
        if (parent == null)
        {
            Debug.LogError("Parent가 설정되지 않았습니다.");
            return;
        }

        // 부모 오브젝트와 그 자식 오브젝트들에서 MeshFilter 컴포넌트를 가져옵니다.
        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();
        // 부모 오브젝트와 그 자식 오브젝트들에서 MeshRenderer 컴포넌트를 가져옵니다.
        MeshRenderer[] meshRenderers = parent.GetComponentsInChildren<MeshRenderer>();

        // MeshFilter 컴포넌트가 없다면 경고 메시지를 출력하고 메서드를 종료합니다.
        if (meshFilters.Length == 0)
        {
            Debug.LogWarning("부모 오브젝트에 MeshFilter가 없습니다.");
            return;
        }

        // CombineInstance 리스트를 생성합니다.
        List<CombineInstance> combineList = new List<CombineInstance>(meshFilters.Length);

        // 각 MeshFilter에 대해 CombineInstance 생성 및 리스트에 추가
        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].sharedMesh != null)
            {
                combineList.Add(new CombineInstance
                {
                    mesh = meshFilters[i].sharedMesh, // 메쉬 데이터
                    transform = meshFilters[i].transform.localToWorldMatrix // 변환 행렬
                });
            }
        }

        // 새로운 게임 오브젝트 생성, 결합된 메쉬를 적용
        GameObject combinedObject = new GameObject(parent.name);
        Transform ppTransform = parent.transform.parent; // pp: parentparent (부모 오브젝트의 상위 오브젝트)

        // parent 오브젝트가 상위 오브젝트를 가지고 있다면 병합된 오브젝트의 부모를 상위 오브젝트로 설정
        if (ppTransform != null)
        {
            combinedObject.transform.parent = ppTransform;
        }
        // parent 오브젝트가 상위 오브젝트를 가지고 있지 않다면 병합된 오브젝트의 부모를 parent로 설정
        else
        {
            combinedObject.transform.parent = parent.transform;
        }

        // 병합된 오브젝트에 MeshFilter와 MeshRenderer 컴포넌트 추가
        var meshFilterComponent = combinedObject.AddComponent<MeshFilter>();
        var meshRendererComponent = combinedObject.AddComponent<MeshRenderer>();

        // 새로운 Mesh 객체 생성
        Mesh combinedMesh = new Mesh();
        // 모든 머터리얼을 수집하고 중복을 제거하여 materials 배열 생성
        Material[] materials = meshRenderers.SelectMany(mr => mr.sharedMaterials).Distinct().ToArray();
        // Mesh.CombineMeshes 메서드를 사용하여 메쉬와 머터리얼을 병합
        combinedMesh.CombineMeshes(combineList.ToArray(), true, true, false);
        // 병합된 메쉬를 MeshFilter 컴포넌트에 할당
        meshFilterComponent.sharedMesh = combinedMesh;

        // 병합된 오브젝트의 MeshRenderer 컴포넌트에 머터리얼 배열 할당
        meshRendererComponent.sharedMaterials = materials;

        // AddMeshCollider가 true인 경우 메쉬 콜라이더를 추가
        if (addMeshCollider)
        {
            var meshCollider = combinedObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = combinedMesh;
        }

        // DeactivateParentAfterMerge가 true인 경우 부모 오브젝트를 비활성화
        if (deactivateParentAfterMerge)
        {
            parent.SetActive(false);
        }

        // DestroyParentAfterMerge가 true인 경우 부모 오브젝트를 삭제
        if (destroyParentAfterMerge)
        {
            Destroy(parent);
        }
    }
}