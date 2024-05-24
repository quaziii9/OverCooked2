using System.Collections;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private GameObject[] gasBurners;
    [SerializeField] private Transform[] playerSpawnPositions; // 플레이어 스폰 위치들
    [SerializeField] private float respawnDelay = 5.0f; // 리스폰 대기 시간

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
            StartCoroutine(RespawnOnBurner(other.gameObject));
        }
    }

    private void PlayerDeath(GameObject player)
    {
        // 플레이어 사망 처리
        Debug.Log("플레이어 사망");
        PlayerPuff.Instance.SpawnPuff(player.transform);
        player.transform.GetChild(0).gameObject.SetActive(false);

        // 플레이어의 진영과 스폰 위치 설정 (예시로, playerName을 통해 진영을 구분한다고 가정)
        int playerIndex = DeterminePlayerIndex(player);
        Transform spawnPosition = playerSpawnPositions[playerIndex];

        // 5초 카운트 UI 활성화 후 스폰 처리
        StartCoroutine(RespawnPlayer(player, spawnPosition));
    }

    private int DeterminePlayerIndex(GameObject player)
    {
        // 플레이어 개체에 따라 인덱스를 결정하는 로직 (예시)
        // 실제 구현 시에는 플레이어의 팀 및 인덱스에 따라 구분할 필요가 있습니다.
        string playerName = player.name;
        if (playerName.Contains("1"))
        {
            return 0;
        }
        else if (playerName.Contains("2"))
        {
            return 1;
        }
        else if (playerName.Contains("3"))
        {
            return 2;
        }
        else // if (playerName.Contains("4"))
        {
            return 3;
        }
    }

    private IEnumerator RespawnPlayer(GameObject player, Transform spawnPosition)
    {
        // 5초 대기
        yield return new WaitForSeconds(respawnDelay);

        // 5초 카운트 UI 애니메이션

        // 연기 발생 및 플레이어 스폰
        PlayerPuff.Instance.SpawnPuff(spawnPosition);
        player.transform.position = spawnPosition.position;

        // 연기가 모두 나오고 스폰해야 하기에 1초 딜레이
        player.transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log("플레이어 리스폰");
    }

    private IEnumerator RespawnOnBurner(GameObject obj)
    {
        // 3초 대기
        yield return new WaitForSeconds(3.0f);

        // 가스 버너의 빈 위치 탐색 및 스폰
        foreach (GameObject burner in gasBurners)
        {
            Transform burnerSpot = burner.transform.GetChild(3);
            if (burnerSpot.childCount == 0)
            {
                obj.transform.position = burnerSpot.position;
                obj.transform.SetParent(burnerSpot);
                Debug.Log("오브젝트 가스 버너에 스폰");
                yield break;
            }
        }

        Debug.Log("빈 가스 버너를 찾을 수 없습니다.");
    }
}
