using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 태그를 가진 오브젝트가 트리거에 닿았는지 확인
        if (other.CompareTag("Player"))
        {
            PlayerDeath(other.gameObject);
        } 
        else
        {
            Destroy(other.gameObject);
        }
    }

    private void PlayerDeath(GameObject player)
    {
        // 플레이어 사망 처리
        Debug.Log("플레이어 사망");
        player.SetActive(false);
    }
}
