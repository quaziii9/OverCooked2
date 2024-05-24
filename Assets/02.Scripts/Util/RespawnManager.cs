using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RespawnManager : Singleton<RespawnManager>
{
    [System.Serializable]
    public struct RespawnUI
    {
        public GameObject playerUI; // Player UI 오브젝트 (Background와 Text를 포함)
        public Text countdownText;  // Text UI 오브젝트
    }

    [SerializeField] private RespawnUI[] respawnUIs; // 리스폰 UI 정보 배열
    [SerializeField] private Transform[] playerSpawnPositions; // 리스폰 위치들
    [SerializeField] private float respawnDelay = 5.0f; // 리스폰 대기 시간

    private void Start()
    {
        if (respawnUIs.Length != playerSpawnPositions.Length)
        {
            Debug.LogError("RespawnUIs와 PlayerSpawnPositions 배열의 크기가 일치해야 합니다.");
            return;
        }

        // 초기에는 모든 Player UI를 비활성화
        foreach (var ui in respawnUIs)
        {
            ui.playerUI.SetActive(false);
        }
    }

    public void StartRespawnCountdown(int spawnIndex)
    {
        if (spawnIndex >= 0 && spawnIndex < respawnUIs.Length)
        {
            StartCoroutine(RespawnCountdownCoroutine(spawnIndex));
        }
        else
        {
            Debug.LogError("잘못된 spawnIndex입니다.");
        }
    }

    private IEnumerator RespawnCountdownCoroutine(int index)
    {
        RespawnUI respawnUI = respawnUIs[index];
        respawnUI.playerUI.SetActive(true);

        for (int i = (int)respawnDelay; i > 0; i--)
        {
            respawnUI.countdownText.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }

        respawnUI.playerUI.SetActive(false);
        RespawnPlayer(index);
    }

    private void RespawnPlayer(int index)
    {
        // 실제로 플레이어를 리스폰시키는 로직을 여기에 추가
        Transform spawnPosition = playerSpawnPositions[index];
        Debug.Log($"플레이어가 {spawnPosition.position}에서 리스폰되었습니다.");
        // 여기서 PlayerPuff.Instance.SpawnPuff 등을 호출할 수 있습니다.
    }
}
