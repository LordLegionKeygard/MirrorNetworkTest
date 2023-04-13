using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class PlayerStatDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText = null;
    [SerializeField] private TMP_Text _scoreText = null;
    public uint PlayerNetId;

    public void SetUp(Player player)
    {
        PlayerNetId = player.netId;

        var gamePlayer = NetworkClient.spawned[PlayerNetId].GetComponent<NetworkGamePlayerBh>();

        _playerNameText.text = gamePlayer.DisplayName;
        _scoreText.text = "-    score: " + gamePlayer.Score.ToString();
    }

    public void UpdateScoreText(int value)
    {
        _scoreText.text = value.ToString();
    }
}
