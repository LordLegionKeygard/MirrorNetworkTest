using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerBh _networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject _landingPagePanel = null;

    public void HostLobby()
    {
        _networkManager.StartHost();

        _landingPagePanel.SetActive(false);
    }
}
