using System.Collections;
using UnityEngine;

public class AniTEST : MonoBehaviour
{
    // 자식 객체의 애니메이터 컴포넌트를 저장할 배열
    private Animator[] childAnimators;
    public GameObject bus;

    private void Awake()
    {
        // 모든 자식 객체의 애니메이터 컴포넌트를 가져옴
        childAnimators = GetComponentsInChildren<Animator>();
    }

    private void Start()
    {
        // 모든 자식 객체의 애니메이터 컴포넌트를 가져옴
        childAnimators = GetComponentsInChildren<Animator>();
        if(gameObject.name=="Level1")
        {
            StartCoroutine(StartFlip());
        }
    }

    private void OnEnable()
    {
        // 모든 자식 객체의 애니메이터 컴포넌트를 가져옴
        childAnimators = GetComponentsInChildren<Animator>();
    }

    private void Update()
    {
        // 특정 조건을 만족하면 모든 자식 객체의 애니메이션을 실행시킴
        if (Input.GetKeyDown(KeyCode.Space)) // 예시로 스페이스바를 누를 때 실행하도록 설정
        {
            StartCoroutine(StartFlip());
        }
    }

    public void PlayChildAnimations()
    {
        StartCoroutine(StartFlip());
    }

    private IEnumerator StartFlip()
    {
        int count = 0;
        
        var busSc = bus.GetComponent<Bus>();
        busSc.enabled = false;
        
        var rb= bus.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        
        foreach (Animator animator in childAnimators)
        {
            count++;
            animator.SetTrigger("flip");
            if (count >= 3)
            {
                yield return null;
            }
        }
        
        yield return null;
        busSc.enabled = true;
        rb.isKinematic = false;
    }
}
