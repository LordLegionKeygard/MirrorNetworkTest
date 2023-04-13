using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class RoundSystem : NetworkBehaviour
{
    [SerializeField] private Animator _animator;

    private NetworkManagerBh _room;
    private NetworkManagerBh Room
    {
        get
        {
            if (_room != null) return _room;
            return _room = NetworkManager.singleton as NetworkManagerBh;
        }
    }

    public override void OnStartServer()
    {
        NetworkManagerBh.OnServerStopped += CleanUpServer;
        NetworkManagerBh.OnServerReadied += CheckToStartRound;
    }


    [ServerCallback]
    public void StartRound()
    {
        RpcStartRound();
    }

    [Server]
    



    [ServerCallback]
    public void EndRound()
    {
        RpcStartRound();
    }

    [Server]
    private void CheckToStartRound(NetworkConnection conn)
    {
        if (Room.GamePlayers.Count(x => x.connectionToClient.isReady) != Room.GamePlayers.Count) return;

        _animator.enabled = true;

        RpcStartCountDown();
    }


    [ClientRpc]
    private void RpcStartCountDown()
    {
        _animator.enabled = true;
    }

    public void CountdownEnded()
    {
        _animator.enabled = false;
    }

    [ClientRpc]
    private void RpcStartRound()
    {
        InputManager.Remove(ActionMapNames.Player);
    }

    [ServerCallback]
    private void OnDestroy() => CleanUpServer();


    [Server]
    private void CleanUpServer()
    {
        NetworkManagerBh.OnServerStopped -= CleanUpServer;
        NetworkManagerBh.OnServerReadied -= CheckToStartRound;
    }
}
