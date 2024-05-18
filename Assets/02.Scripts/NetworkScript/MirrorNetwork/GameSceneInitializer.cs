using Mirror;
using UnityEngine;

public class GameSceneInitializer : MonoBehaviour
{
    // Branch Flag
    private void Start()
    {
        CustomNetworkManager networkManager = (CustomNetworkManager)NetworkManager.singleton;

        if (networkManager != null)
        {
            networkManager.autoStartServer = true; // 서버로 시작할 경우 true로 설정
            // networkManager.autoStartClient = true; // 클라이언트로 시작할 경우 true로 설정
            networkManager.StartGameDirectly();
        }
    }
}
