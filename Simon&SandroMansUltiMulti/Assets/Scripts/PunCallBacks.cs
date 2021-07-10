using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PunCallBacks : MonoBehaviourPunCallbacks
{
    public static PunCallBacks Instance;

    private NetworkUIManager uiManager;

    private List<RoomInfo> roomInfos;
    public List<RoomInfo> RoomInfos { get => roomInfos; set => roomInfos = value; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
 
    public void Start()
    {
        uiManager = NetworkUIManager.Instance;
        uiManager.SetConnectionStatus(ConnectionStatus.Connecting);

        roomInfos = new List<RoomInfo>();
    }

    public override void OnConnected()
    {
        Debug.Log("Connected");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("ConnectedToMaster");
        uiManager.SetConnectionStatus(ConnectionStatus.Connected);
    }
    public override void OnJoinedLobby()
    {
        uiManager.SetConnectionStatus(ConnectionStatus.HostingOrJoiningRoom);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log(roomList.Count);
        roomInfos = roomList;
        uiManager.UpdateRoomUI_Texts();
    }
    public override void OnCreatedRoom()
    {
        uiManager.SetConnectionStatus(ConnectionStatus.InRoom);
        uiManager.UpdatePlayerDescriptionTexts();
    }
    public override void OnJoinedRoom()
    {
        uiManager.SetConnectionStatus(ConnectionStatus.InRoom);
        uiManager.UpdatePlayerDescriptionTexts();
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        uiManager.SetPlayerMessageText("Room Full!");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        uiManager.UpdatePlayerDescriptionTexts();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(!PhotonNetwork.LocalPlayer.IsMasterClient)
        uiManager.UpdatePlayerDescriptionTexts();
    }
}
