using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerStatsDisplay : MonoBehaviour
{
    public static PlayerStatsDisplay Instance;
    [SerializeField] private PlayerStatDisplay _statDisplay = null;
    [SerializeField] private Transform _statHolderTransform = null;
    public readonly List<PlayerStatDisplay> StatDisplays = new List<PlayerStatDisplay>();

    private void Awake()
    {
        Instance = this;

        Player.OnPlayerSpawned += HandlePlayerSpawned;
        Player.OnPlayerDespawned += HandlePlayerDespawned;
    }

    private void OnDestroy()
    {
        Player.OnPlayerSpawned -= HandlePlayerSpawned;
        Player.OnPlayerDespawned -= HandlePlayerDespawned;
    }

    private void HandlePlayerSpawned(Player player)
    {
        PlayerStatDisplay displayInstance = Instantiate(_statDisplay, _statHolderTransform);

        displayInstance.SetUp(player);

        StatDisplays.Add(displayInstance);
    }

    private void HandlePlayerDespawned(Player player)
    {
        PlayerStatDisplay displayInstance = StatDisplays.FirstOrDefault(x => x.PlayerNetId == player.netId);

        if (displayInstance == null) return;

        StatDisplays.Remove(displayInstance);

        Destroy(displayInstance.gameObject);
    }
}
