using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MyNetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject serverMenuPanel;

    [Header("UI Elements Menu")]
    [SerializeField] private TMP_InputField nameText;
    [SerializeField] private GameObject missingNameText;

    [Header("RoomSettings")]
    [SerializeField] private int roomSize = 4;
    [SerializeField] private bool isVisible = true;
    RoomOptions roomOptions;
    private void Awake()
    {
        mainMenuPanel.SetActive(false);
        serverMenuPanel.SetActive(false);

        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting");
    }

    public void JoinLobby()
    {
        if (string.IsNullOrWhiteSpace(nameText.text))
        {
            missingNameText.SetActive(true);
            return;
        }

        serverMenuPanel.SetActive(true);
        mainMenuPanel.SetActive(false);

        PhotonNetwork.JoinRandomRoom();

        PhotonNetwork.LocalPlayer.NickName = nameText.text;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Couldn't locate opened Room");
        Debug.Log("Lets create one");

        roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSize;
        roomOptions.IsVisible = isVisible;

        PhotonNetwork.CreateRoom(string.Empty, roomOptions);
    }
    public override void OnLeftRoom()
    {
        mainMenuPanel.SetActive(true);
    }
    public override void OnConnectedToMaster()
    {
        mainMenuPanel.SetActive(true);
        Debug.Log("We Are Connected to a Master Server");

    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        serverMenuPanel.SetActive(false);
        missingNameText.SetActive(false);
    }

}
