using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    [Header("Server/Client Settings")]
    public string networkAddress_local = "localhost";

    [Header("UI Elements")]
    public GameObject[] playerSlots; // 플레이어 슬롯 UI 배열

    #region StartServerManually / StartClientManually / StopBattle
    public void StartServerManually()
    {
        StartServer();
    }

    public void StartClientManually()
    {
        networkAddress = networkAddress_local;
        StartClient();
    }
    

    public void StopBattle()
    {
        base.StopHost();

        if (NetworkServer.active)
        {
            StopServer();
        }
        if (NetworkClient.isConnected)
        {
            StopClient();
        }

        // 슬롯 상태 초기화
        UpdatePlayerSlots();
    }
    #endregion

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        UpdatePlayerSlots();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        UpdatePlayerSlots();
    }

    private void UpdatePlayerSlots()
    {
        // 모든 플레이어 슬롯을 초기화 (비활성화)
        foreach (GameObject slot in playerSlots)
        {
            slot.SetActive(false);
        }

        // 활성화된 플레이어 수에 따라 슬롯 활성화
        int index = 0;
        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            if (index < playerSlots.Length)
            {
                playerSlots[index].SetActive(true);
                index++;
            }
        }
    }
}