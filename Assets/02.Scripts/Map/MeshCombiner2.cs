using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering; // 이 네임스페이스를 포함해야 합니다

public class MeshCombiner2 : MonoBehaviour
{
    public GameObject Parent;
    public Material Material;
    public bool DeactivateParentAfterMerge = true;
    public bool DestroyParentAfterMerge = false;

    [ContextMenu("Merge")]
    public void MergeMeshes()
    {
        MeshFilter[] meshFilters = Parent.GetComponentsInChildren<MeshFilter>();
        List<CombineInstance> combineList = new List<CombineInstance>();

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].sharedMesh != null)
            {
                CombineInstance combineInstance = new CombineInstance
                {
                    mesh = meshFilters[i].sharedMesh,
                    transform = meshFilters[i].transform.localToWorldMatrix
                };
                combineList.Add(combineInstance);
            }
        }

        GameObject combinedObject = new GameObject("Combined Mesh");
        combinedObject.AddComponent<MeshFilter>();
        combinedObject.AddComponent<MeshRenderer>();

        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = IndexFormat.UInt32; // 인덱스 형식을 UInt32로 설정

        combinedMesh.CombineMeshes(combineList.ToArray());
        combinedObject.GetComponent<MeshFilter>().sharedMesh = combinedMesh;
        combinedObject.GetComponent<MeshRenderer>().material = Material;

        if (DeactivateParentAfterMerge)
        {
            Parent.SetActive(false);
        }

        if (DestroyParentAfterMerge)
        {
            Destroy(Parent);
        }
    }
}