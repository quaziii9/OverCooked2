using UnityEngine;
using Mirror;

/*
    문서: https://mirror-networking.gitbook.io/docs/components/network-room-manager
    API 참조: https://mirror-networking.com/docs/api/Mirror.NetworkRoomManager.html

    참조: NetworkManager
    문서: https://mirror-networking.gitbook.io/docs/components/network-manager
    API 참조: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

/// <summary>
/// 네트워크 룸을 포함한 특수한 NetworkManager입니다.
/// 룸에는 참가한 플레이어를 추적하는 슬롯과 최대 플레이어 수가 있습니다.
/// 룸 플레이어 객체에는 NetworkRoomPlayer 컴포넌트가 필요합니다.
/// NetworkRoomManager는 NetworkManager에서 파생되었으므로 NetworkManager 클래스에서 제공하는 많은 가상 함수를 구현합니다.
/// </summary>
public class OverNetworkRoomManager : NetworkRoomManager
{
    #region 서버 콜백

    /// <summary>
    /// 서버가 시작될 때 호출됩니다 - 호스트가 시작될 때도 포함됩니다.
    /// </summary>
    public override void OnRoomStartServer() { }

    /// <summary>
    /// 서버가 중지될 때 호출됩니다 - 호스트가 중지될 때도 포함됩니다.
    /// </summary>
    public override void OnRoomStopServer() { }

    /// <summary>
    /// 호스트가 시작될 때 호출됩니다.
    /// </summary>
    public override void OnRoomStartHost() { }

    /// <summary>
    /// 호스트가 중지될 때 호출됩니다.
    /// </summary>
    public override void OnRoomStopHost() { }

    /// <summary>
    /// 새로운 클라이언트가 서버에 연결될 때 호출됩니다.
    /// </summary>
    /// <param name="conn">새로운 연결.</param>
    public override void OnRoomServerConnect(NetworkConnectionToClient conn) { }

    /// <summary>
    /// 클라이언트가 연결을 끊을 때 호출됩니다.
    /// </summary>
    /// <param name="conn">연결이 끊어진 클라이언트.</param>
    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn) { }

    /// <summary>
    /// 네트워크 씬 로딩이 완료되었을 때 서버에서 호출됩니다.
    /// </summary>
    /// <param name="sceneName">새로운 씬의 이름.</param>
    public override void OnRoomServerSceneChanged(string sceneName) { }

    /// <summary>
    /// 서버에서 룸 플레이어 객체를 생성하는 방법을 커스터마이징할 수 있습니다.
    /// <para>기본적으로 roomPlayerPrefab이 사용되지만, 이 함수를 통해 해당 동작을 커스터마이징할 수 있습니다.</para>
    /// </summary>
    /// <param name="conn">플레이어 객체의 연결.</param>
    /// <returns>새로운 룸 플레이어 객체.</returns>
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        return base.OnRoomServerCreateRoomPlayer(conn);
    }

    /// <summary>
    /// 서버에서 게임 플레이어 객체를 생성하는 방법을 커스터마이징할 수 있습니다.
    /// <para>기본적으로 gamePlayerPrefab이 사용되지만, 이 함수를 통해 해당 동작을 커스터마이징할 수 있습니다. 함수에서 반환된 객체는 연결된 룸 플레이어를 대체하는 데 사용됩니다.</para>
    /// </summary>
    /// <param name="conn">플레이어 객체의 연결.</param>
    /// <param name="roomPlayer">이 연결의 룸 플레이어 객체.</param>
    /// <returns>새로운 게임 플레이어 객체.</returns>
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        return base.OnRoomServerCreateGamePlayer(conn, roomPlayer);
    }

    /// <summary>
    /// 서버에서 게임 플레이어 객체를 생성하는 방법을 커스터마이징할 수 있습니다.
    /// <para>이 함수는 첫 번째 게임 플레이 씬 이후의 추가적인 씬에서만 호출됩니다.</para>
    /// <para>초기 게임 플레이 씬의 플레이어 객체를 커스터마이징하려면 OnRoomServerCreateGamePlayer를 참조하세요.</para>
    /// </summary>
    /// <param name="conn">플레이어 객체의 연결.</param>
    public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnRoomServerAddPlayer(conn);
    }

    /// <summary>
    /// 클라이언트가 룸 씬에서 게임 플레이 씬으로 전환을 완료했을 때 서버에서 호출됩니다.
    /// <para>룸에서 전환할 때, 룸 플레이어는 게임 플레이어 객체로 교체됩니다. 이 콜백 함수는 룸 플레이어의 상태를 게임 플레이어 객체에 적용할 기회를 제공합니다.</para>
    /// </summary>
    /// <param name="conn">플레이어의 연결</param>
    /// <param name="roomPlayer">룸 플레이어 객체.</param>
    /// <param name="gamePlayer">게임 플레이어 객체.</param>
    /// <returns>룸 플레이어를 교체하는 것을 허용하지 않으려면 false를 반환.</returns>
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }

    /// <summary>
    /// 클라이언트가 준비 상태를 변경할 때 NetworkRoomPlayer.CmdChangeReadyState에서 서버로부터 호출됩니다.
    /// </summary>
    public override void ReadyStatusChanged()
    {
        base.ReadyStatusChanged();
    }

    /// <summary>
    /// 룸에 있는 모든 플레이어가 준비되었을 때 서버에서 호출됩니다.
    /// <para>기본 구현은 ServerChangeScene()을 사용하여 게임 플레이어 씬으로 전환합니다. 이 콜백을 구현하면 모든 플레이어가 준비되었을 때 카운트다운이나 그룹 리더의 확인 등 맞춤형 동작을 할 수 있습니다.</para>
    /// </summary>
    public override void OnRoomServerPlayersReady()
    {
        base.OnRoomServerPlayersReady();
    }

    /// <summary>
    /// CheckReadyToBegin에서 플레이어가 준비되지 않았을 때 서버에서 호출됩니다.
    /// <para>준비되지 않은 플레이어가 계속 참여할 때 여러 번 호출될 수 있습니다.</para>
    /// </summary>
    public override void OnRoomServerPlayersNotReady() { }

    #endregion
     
    #region 클라이언트 콜백

    /// <summary>
    /// 게임 클라이언트가 룸에 들어갈 때 사용자 정의 동작을 허용하는 훅입니다.
    /// </summary>
    public override void OnRoomClientEnter() { }

    /// <summary>
    /// 게임 클라이언트가 룸을 나갈 때 사용자 정의 동작을 허용하는 훅입니다.
    /// </summary>
    public override void OnRoomClientExit() { }

    /// <summary>
    /// 클라이언트가 서버에 연결될 때 호출됩니다.
    /// </summary>
    public override void OnRoomClientConnect() { }

    /// <summary>
    /// 클라이언트가 서버와의 연결이 끊어졌을 때 호출됩니다.
    /// </summary>
    public override void OnRoomClientDisconnect() { }

    /// <summary>
    /// 클라이언트가 시작될 때 호출됩니다.
    /// </summary>
    public override void OnRoomStartClient() { }

    /// <summary>
    /// 클라이언트가 중지될 때 호출됩니다.
    /// </summary>
    public override void OnRoomStopClient() { }

    /// <summary>
    /// 클라이언트가 새로운 네트워크 씬 로딩을 완료했을 때 호출됩니다.
    /// </summary>
    public override void OnRoomClientSceneChanged() { }

    #endregion

    #region 선택적 UI

    public override void OnGUI()
    {
        base.OnGUI();
    }

    #endregion
}
