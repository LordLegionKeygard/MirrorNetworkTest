using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class ScoreController : NetworkBehaviour
{
    public static event Action OnPlayerWin;

    private int _currentScore;

    public void ChangeScore()
    {
        _currentScore += 1;
        // CheckWinner();
        UpdateText();
    }

    private void CheckWinner()
    {
        // if(_currentScore >= 1)
        // {
        //     CmdWin();
        // }
    }

    private void UpdateText()
    {
        // for (int i = 0; i < PlayerStatsDisplay.Instance.StatDisplays.Count; i++)
        // {
        //     if (PlayerStatsDisplay.Instance.StatDisplays[i].DisplayName == _playerName.Name)
        //     {
        //         PlayerStatsDisplay.Instance.StatDisplays[i].UpdateScoreText(_currentScore);
        //     }
        // }
    }

    // [Command]
    // private void CmdWin()
    // {
    //     RpcOnPlayerWin();
    // }

    // [ClientRpc]
    // private void RpcOnPlayerWin()
    // {
    //     OnPlayerWin?.Invoke();
    // }
}
