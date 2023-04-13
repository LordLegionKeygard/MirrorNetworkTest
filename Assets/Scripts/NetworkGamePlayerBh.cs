using UnityEngine;
using Mirror;

public class NetworkGamePlayerBh : NetworkBehaviour
{
    [SyncVar]
    private string _displayName = "Loading...";

    public string DisplayName => _displayName;

    [SyncVar]
    private int _score;

    public int Score => _score;

    private NetworkManagerBh _room;

    private NetworkManagerBh Room
    {
        get
        {
            if (_room != null) return _room;
            return _room = NetworkManager.singleton as NetworkManagerBh;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this._displayName = displayName;
    }

    // [Server]
    // public void IncrementScore()
    // {
    //     _score++;
    // }
}
