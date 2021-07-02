using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MyNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private bool startGameOffline;
 
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject roomsPanel;
    
    [Header("UI Elements Menu")]
    [SerializeField] private TMP_InputField nameText;
    [SerializeField] private GameObject missingNameText;
    
    //[Header("RoomSettings")]
    //[SerializeField] private int roomSize = 4;
    //[SerializeField] private bool isVisible = true;

    private void Awake()
    {
        mainMenuPanel.SetActive(false);
        roomsPanel.SetActive(false);

        if (!PhotonNetwork.IsConnected || startGameOffline)
        {
            Debug.Log("startinggameoffline"); 
            PhotonNetwork.OfflineMode = true;
        }

        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting");
    }


    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
        {
            mainMenuPanel.SetActive(true);
        }
        Debug.Log("We Are Connected to a Master Server");

    }
    public void JoinLobby()
    {
        if (string.IsNullOrWhiteSpace(nameText.text))
        {
            missingNameText.SetActive(true);
            return;
        }

        roomsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = nameText.text;
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        // mainMenuPanel.SetActive(true);

        Debug.Log("Joined Lobby");
    }
}
