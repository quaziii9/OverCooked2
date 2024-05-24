using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RespawnManager : Singleton<RespawnManager>
{
    [SerializeField] private GameObject[] countdown;
    [SerializeField] private Transform[] playerSpawnPositions; // 리스폰 위치들
    [SerializeField] private float respawnDelay = 5.0f; // 리스폰 대기 시간

    private void Start()
    {
        if (countdown.Length != playerSpawnPositions.Length)
        {
            Debug.LogError("CountdownTexts와 PlayerSpawnPositions 배열의 크기가 일치해야 합니다.");
            return;
        }

        // 초기에는 모든 UI를 비활성화
        foreach (var obj in countdown)
        {
            obj.gameObject.SetActive(false);
        }
    }

    public void StartRespawnCountdown(GameObject player, int spawnIndex)
    {
        if (spawnIndex >= 0 && spawnIndex < countdown.Length)
        {
            StartCoroutine(RespawnCountdownCoroutine(player, spawnIndex));
        }
        else
        {
            Debug.LogError("잘못된 spawnIndex입니다.");
        }
    }

    private IEnumerator RespawnCountdownCoroutine(GameObject player, int index)
    {
        GameObject obj = countdown[index];
        Text countdownText = obj.transform.GetChild(1).GetComponent<Text>();

        obj.SetActive(true);

        for (int i = (int)respawnDelay; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }

        obj.SetActive(false);
        RespawnPlayer(player, index);
    }

    private void RespawnPlayer(GameObject player, int index)
    {
        Transform spawnPosition = playerSpawnPositions[index];
        Debug.Log($"플레이어가 {spawnPosition.position}에서 리스폰되었습니다.");

        // 연기 발생 및 플레이어 스폰
        PlayerPuff.Instance.SpawnPuff(spawnPosition);
        player.transform.position = spawnPosition.position;

        // 연기가 모두 나오고 스폰해야 하기에 1초 딜레이
        player.transform.GetChild(0).gameObject.SetActive(true);
    }
}
