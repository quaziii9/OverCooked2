using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    [Header("Server Setting")]
    public bool autoStartServer = false;
    public bool autoStartClient = false;
    public string serverAddress = "localhost";

    [Header("Player Prefabs")]
    public GameObject[] playerAObjects; // 플레이어 A가 조작할 오브젝트들
    public GameObject[] playerBObjects; // 플레이어 B가 조작할 오브젝트들

    public override void Awake()
    {
        base.Awake();

        if (autoStartServer)
        {
            StartServer();
        }
        else if (autoStartClient)
        {
            networkAddress = this.serverAddress;
            StartClient();
        }
    }

    public void StartGameDirectly()
    {
        if (autoStartServer)
        {
            StartServer();
        }
        else if (autoStartClient)
        {
            networkAddress = this.serverAddress;
            StartClient();
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        if (numPlayers == 1)
        {
            // 첫 번째 플레이어 (A)
            foreach (var obj in playerAObjects)
            {
                AssignOwnership(conn, obj);
            }
        }
        else if (numPlayers == 2)
        {
            // 두 번째 플레이어 (B)
            foreach (var obj in playerBObjects)
            {
                AssignOwnership(conn, obj);
            }
        }
    }

    void AssignOwnership(NetworkConnectionToClient conn, GameObject obj)
    {
        NetworkIdentity networkIdentity = obj.GetComponent<NetworkIdentity>();
        if (networkIdentity != null)
        {
            networkIdentity.AssignClientAuthority(conn);
        }
    }
}
