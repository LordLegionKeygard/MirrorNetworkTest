using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerBh : NetworkManager
{
    [SerializeField] private int _minPlayers = 2;
    private string _menuScene = ScenesEnum.MainMenu.ToString();

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerBh _roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerBh _gamePlayerPrefab = null;
    [SerializeField] private GameObject _playerSpawnSystem = null;
    [SerializeField] private GameObject _roundSystem = null;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;

    public List<NetworkRoomPlayerBh> RoomPlayers { get; } = new List<NetworkRoomPlayerBh>();
    public List<NetworkGamePlayerBh> GamePlayers { get; } = new List<NetworkGamePlayerBh>();

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().name != _menuScene)
        {
            Debug.Log(_menuScene);
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {

        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerBh>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == _menuScene)
        {
            bool _isLeader = RoomPlayers.Count == 0;

            NetworkRoomPlayerBh roomPlayerInstance = Instantiate(_roomPlayerPrefab);

            roomPlayerInstance.IsLeader = _isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();

        RoomPlayers.Clear();
        GamePlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < _minPlayers) return false;

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) return false;
        }

        return true;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == _menuScene)
        {
            if (!IsReadyToStart()) return;

            ServerChangeScene(ScenesEnum.Game.ToString());
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (SceneManager.GetActiveScene().name == _menuScene && newSceneName.StartsWith(ScenesEnum.Game.ToString()))
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gamePlayerInstance = Instantiate(_gamePlayerPrefab);
                gamePlayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
            }
        }

        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith(ScenesEnum.Game.ToString()))
        {
            GameObject playerSpawnSystemInstance = Instantiate(_playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);

            GameObject roundSystemInstance = Instantiate(_roundSystem);
            NetworkServer.Spawn(roundSystemInstance);
        }
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }
}

