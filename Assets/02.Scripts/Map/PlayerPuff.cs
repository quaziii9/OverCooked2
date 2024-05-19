using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerPuff : Singleton<PlayerPuff>
{

    public GameObject puffWalkPrefab;   // 걷는 퍼프 프리팹
    public GameObject puffBustPrefab;   // 버스트 퍼프 프리팹
    private IObjectPool<Puff> walkPool; // 걷는 퍼프 풀
    private IObjectPool<Puff> bustPool; // 버스트 퍼프 풀
    //public Transform puffPostion;
    private void Awake()
    {
        walkPool = new ObjectPool<Puff>(CreateWalk, OnGetPuff, OnReleasePuff, OnDestroyPuff, maxSize: 1000);
        bustPool = new ObjectPool<Puff>(CreateBust, OnGetPuff, OnReleasePuff, OnDestroyPuff, maxSize: 1000);
    }


    // 아래부터는 퍼프 관련 함수
    public void BoostPuff(Transform puffPostion) // 부스트 퍼프 실행
    {
        var bust = bustPool.Get();
        bust.transform.position = puffPostion.transform.position;
        //bust.transform.rotation = puffPostion.transform.rotation;
    }
    public void MovePuff(Transform puffPostion) //무브 퍼프 실행
    {
        var walk = walkPool.Get();
        walk.transform.position = puffPostion.transform.position;
        //walk.transform.rotation=puffPostion.transform.rotation;
    }
    private Puff CreateWalk() // 워크퍼프 생성 후 풀에 담음
    {
        Puff puff = Instantiate(puffWalkPrefab).GetComponent<Puff>();
        puff.SetManagedPool(walkPool);
        puff.transform.localScale = Vector3.one*2;
        return puff;
    }
    private Puff CreateBust() // 버스트 퍼프 생성
    {
        Puff puff = Instantiate(puffBustPrefab).GetComponent<Puff>();
        puff.SetManagedPool(bustPool);
        puff.transform.localScale = Vector3.one*2;
        return puff;
    }

    private void OnGetPuff(Puff puff) // 퍼프를 불러옴
    {
        puff.gameObject.SetActive(true);
    }

    private void OnReleasePuff(Puff puff) // 퍼프 비활성화
    {
        puff.gameObject.SetActive(false);
    }

    private void OnDestroyPuff(Puff puff) // 퍼프 삭제
    {
        Destroy(puff.gameObject);
    }
}
