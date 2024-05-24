using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private GameObject[] gasBurners;

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 태그를 가진 오브젝트가 트리거에 닿았는지 확인
        if (other.CompareTag("Player"))
        {
            PlayerDeath(other.gameObject);
        } 
        else if (other.CompareTag("Ingredient"))
        {
            Destroy(other.gameObject);  // 요리 재료 파괴
        } 
        else
        {
            // 3초 후 가스 버너 위에 스폰
            // 아무 것도 부착되어 있지 않는 가스 버너들 중 랜덤한 위치에 부착
            // 가스 버너의 부착 여부는 가스 버너 오브젝트의 자식 오브젝트 중 4번째 오브젝트가 있는지 없는지 확인해야함
        }
    }

    private void PlayerDeath(GameObject player)
    {
        // 플레이어 사망 처리
        Debug.Log("플레이어 사망");
        player.transform.GetChild(0).gameObject.SetActive(false);
        // 개인 플레이(2개 캐릭터 컨트롤 시) 사망하면 캐릭터 전환
        // 플레이어 스폰 위치는 총 4곳, 진영(레드, 블루) 별로 2곳을 지정, 플레이어 개체 별로 지정된 위치에서 스폰
        // 위치는 public으로 transform 4개 받음, 0, 1번 인덱스는 레드팀 플레이어1, 플레이어2 스폰 위치, 2, 3번 인덱스는 블루팀 플레이어1, 플레이어2 스폰 위치
        // 플레이어 스폰 위치에 5초 카운트 UI 활성화 후 카운트 종료 시 연기 발생하며 플레이어 스폰

    }
}
