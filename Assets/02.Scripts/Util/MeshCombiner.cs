using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    public List<GameObject> objectsToMerge = new List<GameObject>(); // 병합할 오브젝트 리스트
    public bool addMeshCollider = false; // 병합된 메쉬에 메쉬 콜라이더를 추가할지 결정하는 변수
    public bool deactivateObjectsAfterMerge = true; // 병합 후 병합된 오브젝트들을 비활성화할지 결정하는 변수
    public bool destroyObjectsAfterMerge = false; // 병합 후 병합된 오브젝트들을 삭제할지 결정하는 변수

    public bool isCombined = false; // 현재 병합 상태를 나타내는 변수
    public bool hasMultipleMaterials = false; // 여러 개의 머테리얼을 가진 오브젝트가 있는지 확인하는 변수

    // 병합 또는 클리어 동작을 수행하는 메서드
    public void MergeOrClearMeshes()
    {
        InitializeObjects(); // 초기화 메서드 호출

        // 병합할 수 없는 경우 경고 메시지 출력 및 상태 업데이트
        if (hasMultipleMaterials)
        {
            Debug.LogWarning("병합할 수 없습니다. 여러 개의 머테리얼을 가진 오브젝트가 포함되어 있습니다.");
            UpdateStateAndButtonLabel(false);
            return;
        }

        // 병합할 오브젝트가 1개 이하인 경우 경고 메시지 출력 및 상태 업데이트
        if (objectsToMerge.Count <= 1)
        {
            Debug.LogWarning(objectsToMerge.Count == 0 ? "병합할 오브젝트가 없습니다." : "1개의 오브젝트는 병합할 수 없습니다.");
            UpdateStateAndButtonLabel(false);
            return;
        }

        MergeMeshes();
        ClearObjects(); // 병합 후 오브젝트 리스트를 클리어
        UpdateStateAndButtonLabel(true);
        Debug.Log("오브젝트가 성공적으로 병합되었습니다.");
    }

    // 오브젝트 리스트를 초기화하는 메서드
    public void InitializeObjects()
    {
        isCombined = false;
        hasMultipleMaterials = ContainsMultipleMaterials(objectsToMerge);
    }

    // 병합할 오브젝트 리스트를 비우는 메서드
    public void ClearObjects()
    {
        objectsToMerge.Clear();
        isCombined = false;
        hasMultipleMaterials = false;
        UpdateStateAndButtonLabel(false);
    }

    // 여러 개의 머테리얼을 가진 오브젝트가 포함되어 있는지 확인하는 메서드
    private bool ContainsMultipleMaterials(List<GameObject> objects)
    {
        foreach (var obj in objects)
        {
            var meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                if (meshRenderer.sharedMaterials.Length > 1)
                {
                    return true; // 하나라도 여러 개의 머테리얼을 가진 경우 true 반환
                }
            }
        }
        return false; // 모든 오브젝트가 단일 머테리얼을 가진 경우 false 반환
    }

    // 병합 상태와 버튼 레이블을 업데이트하는 메서드
    private void UpdateStateAndButtonLabel(bool combinedState)
    {
        isCombined = combinedState;
        UpdateButtonLabel?.Invoke(isCombined);
        isCombined = false;
    }

    // 버튼 레이블 업데이트를 위한 델리게이트 및 이벤트 정의
    public delegate void ButtonLabelUpdate(bool isCombined);
    public static event ButtonLabelUpdate UpdateButtonLabel;

    // 메쉬를 병합하는 메서드
    private void MergeMeshes()
    {
        var meshFilters = new List<MeshFilter>();
        var meshRenderers = new List<MeshRenderer>();

        // 각 오브젝트에서 MeshFilter와 MeshRenderer 컴포넌트를 가져옴
        foreach (var obj in objectsToMerge)
        {
            meshFilters.AddRange(obj.GetComponentsInChildren<MeshFilter>());
            meshRenderers.AddRange(obj.GetComponentsInChildren<MeshRenderer>());
        }

        // MeshFilter가 없으면 경고 메시지 출력 및 종료
        if (meshFilters.Count == 0)
        {
            Debug.LogWarning("병합할 오브젝트가 없습니다.");
            return;
        }

        // CombineInstance 리스트 생성
        var combineList = CreateCombineInstanceList(meshFilters);

        // 병합된 오브젝트 생성 및 설정
        var combinedObject = CreateCombinedObject(combineList, meshRenderers);

        // 필요한 경우 메쉬 콜라이더 추가
        if (addMeshCollider)
        {
            AddMeshCollider(combinedObject);
        }

        // 필요한 경우 병합된 오브젝트 비활성화
        if (deactivateObjectsAfterMerge)
        {
            DeactivateObjects(objectsToMerge);
        }

        // 필요한 경우 병합된 오브젝트 삭제
        if (destroyObjectsAfterMerge)
        {
            DestroyObjects(objectsToMerge);
        }
    }

    // MeshFilter 리스트에서 CombineInstance 리스트를 생성하는 메서드
    private List<CombineInstance> CreateCombineInstanceList(List<MeshFilter> meshFilters)
    {
        return meshFilters
            .Where(mf => mf.sharedMesh != null)
            .Select(mf => new CombineInstance
            {
                mesh = mf.sharedMesh,
                transform = mf.transform.localToWorldMatrix
            }).ToList();
    }

    // 병합된 오브젝트를 생성하고 설정하는 메서드
    private GameObject CreateCombinedObject(List<CombineInstance> combineList, List<MeshRenderer> meshRenderers)
    {
        var combinedObject = new GameObject("Combined Mesh")
        {
            isStatic = true // 생성된 오브젝트를 정적 오브젝트로 설정
        };
        var meshFilter = combinedObject.AddComponent<MeshFilter>();
        var meshRenderer = combinedObject.AddComponent<MeshRenderer>();

        var combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineList.ToArray(), true, true, false);

        meshFilter.sharedMesh = combinedMesh;
        meshRenderer.sharedMaterials = meshRenderers.SelectMany(mr => mr.sharedMaterials).Distinct().ToArray();

        return combinedObject;
    }

    // 병합된 오브젝트에 메쉬 콜라이더를 추가하는 메서드
    private void AddMeshCollider(GameObject combinedObject)
    {
        var meshCollider = combinedObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = combinedObject.GetComponent<MeshFilter>().sharedMesh;
    }

    // 오브젝트 리스트의 오브젝트들을 비활성화하는 메서드
    private void DeactivateObjects(List<GameObject> objects)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }
    }

    // 오브젝트 리스트의 오브젝트들을 삭제하는 메서드
    private void DestroyObjects(List<GameObject> objects)
    {
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }
}
