using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MyGameManager : MonoBehaviourPun, IPunObservable
{
    private MyPlayer localPlayer;
    public MyPlayer LocalPlayer { get => localPlayer; set => localPlayer = value; }
    private int localPlayerNumber;
    public int LocalPlayerNumber { get => localPlayerNumber; set => localPlayerNumber = value; }
    private MyUI_Handler uiHandler;
     private List<MyPlayer> players;
    
    void Start()
    {
        players = new List<MyPlayer>();
        uiHandler = MyUI_Handler.Instance;
        if(photonView.IsMine)
        {
            uiHandler.GameManager = this;
        }
    }

    public void UpdatePlayerText(int number, string name)
    {
        photonView.RPC("UpdatePlayerTextUI", RpcTarget.All, number, name);
        localPlayerNumber = number;
    }
    [PunRPC]
    public void UpdatePlayerTextUI(int number, string name)
    {
        uiHandler.PlayerButtontexts[number].text = name;
    }
    void Update()
    {
        
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }
    public void StartGame()
    {
        photonView.RPC("UpdatePanels", RpcTarget.All);
    }
    [PunRPC]
    public void UpdatePanels()
    {
        uiHandler.UpdatePanelsOnStart();
    }
    public void Subscribe(MyPlayer player)
    {
        if (players.Contains(player))
            return;

        players.Add(player);  
    }
    public void UnSubscribe(MyPlayer player)
    {
        if (!players.Contains(player))
            return;

        players.Remove(player);
    }
}
