using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Player : NetworkBehaviour
{
    public static event Action<Player> OnPlayerSpawned;
    public static event Action<Player> OnPlayerDespawned;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        OnPlayerSpawned?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnPlayerDespawned?.Invoke(this);
    }
}
