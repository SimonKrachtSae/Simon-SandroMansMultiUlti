using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    private MyUI_Handler uiManager;
    void Start()
    {
        uiManager = MyUI_Handler.Instance;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.OfflineMode = true;
        }
        else
        {
            PhotonNetwork.Instantiate("GameManager", Vector3.zero, Quaternion.identity);
        }
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        uiManager.StartButton.SetActive(true);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.CreateRoom("OfflineRoom");
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LocalPlayer.NickName = "OfflineJeff";
        uiManager.TeamSelectPanel.SetActive(true);
        PhotonNetwork.Instantiate("GameManager", Vector3.zero, Quaternion.identity);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
    }
    void Update()
    {
        
    }
}
