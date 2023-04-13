using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab = null;
    private static List<Transform> _spawnPoints = new List<Transform>();

    public static void AddSpawnPoint(Transform transform)
    {
        _spawnPoints.Add(transform);

        _spawnPoints = _spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform) => _spawnPoints.Remove(transform);

    public override void OnStartClient()
    {
        InputManager.Add(ActionMapNames.Player);
        InputManager.Controls.Player.Look.Enable();
    }

    public override void OnStartServer()
    {
        NetworkManagerBh.OnServerReadied += SpawnPlayer;
    }

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        var rnd = UnityEngine.Random.Range(0, _spawnPoints.Count);

        Transform spawnPoint = _spawnPoints[rnd];

        GameObject playerInstance = Instantiate(_playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.Spawn(playerInstance, conn);

        _spawnPoints.Remove(_spawnPoints[rnd]);
    }

    private void OnDestroy()
    {
        if (!isServer) return;
        NetworkManagerBh.OnServerReadied -= SpawnPlayer;
    }
}
