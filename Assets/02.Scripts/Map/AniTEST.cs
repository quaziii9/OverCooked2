using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AniTEST : MonoBehaviour
{
    // 자식 객체의 애니메이터 컴포넌트를 저장할 배열
    private Animator[] childAnimators;
    void Awake()
    {
        // 모든 자식 객체의 애니메이터 컴포넌트를 가져옴
        childAnimators = GetComponentsInChildren<Animator>();
    }
    void Start()
    {
        // 모든 자식 객체의 애니메이터 컴포넌트를 가져옴
        childAnimators = GetComponentsInChildren<Animator>();
    }
    void OnEnable()
    {
        // 모든 자식 객체의 애니메이터 컴포넌트를 가져옴
        childAnimators = GetComponentsInChildren<Animator>();
    }

    void Update()
    {
        // 특정 조건을 만족하면 모든 자식 객체의 애니메이션을 실행시킴
        if (Input.GetKeyDown(KeyCode.Space)) // 예시로 스페이스바를 누를 때 실행하도록 설정
        {
            //PlayChildAnimations();
            StartCoroutine(start());
        }
    }

    public void PlayChildAnimations()
    {
        foreach (Animator animator in childAnimators)
        {
            animator.SetTrigger("flip");
        }
        
    }
    IEnumerator start()
    {
        foreach (Animator animator in childAnimators)
        {
            animator.SetTrigger("flip");
            
        }
        yield return null;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Bus"))
    //    {
    //        foreach (Animator animator in childAnimators)
    //        {
    //            animator.SetTrigger("flip");
    //        }
    //    }
    //}
    public void Combine()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();
        mf.mesh.CombineMeshes(combine);

        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
        mr.sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;

        gameObject.SetActive(true);
    }
}
