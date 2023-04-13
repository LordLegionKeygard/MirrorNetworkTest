using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayerBh : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _lobbyUI = null;
    [SerializeField] private TMP_Text[] _playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] _playerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button _startGameButton = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool _isLeader;
    public bool IsLeader
    {
        set
        {
            _isLeader = value;
            _startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerBh _room;
    private NetworkManagerBh Room
    {
        get
        {
            if (_room != null) return _room;
            return _room = NetworkManager.singleton as NetworkManagerBh;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);

        _lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);

        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    private void UpdateDisplay()
    {
        if (!isOwned)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.isOwned)
                {
                    player.UpdateDisplay();
                    break;
                }

                return;
            }
        }

        for (int i = 0; i < _playerNameTexts.Length; i++)
        {
            _playerNameTexts[i].text = "Waiting For Player...";
            _playerReadyTexts[i].text = string.Empty;
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            _playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            _playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                    "<color=green>Ready</color>" :
                    "<color=red>Not Ready</color>";
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!_isLeader) return;
        _startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) return;
        
        Room.StartGame();
    }
}
