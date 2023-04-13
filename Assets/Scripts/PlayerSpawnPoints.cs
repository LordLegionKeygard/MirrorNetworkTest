using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoints : MonoBehaviour
{
    private void Awake()
    {
        PlayerSpawnSystem.AddSpawnPoint(transform);
    }

    private void OnDestroy()
    {
        PlayerSpawnSystem.RemoveSpawnPoint(transform);
    }
}
