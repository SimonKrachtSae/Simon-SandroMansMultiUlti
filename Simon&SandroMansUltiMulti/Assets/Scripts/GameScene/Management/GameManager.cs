using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun, IPunObservable
{
	public List<PlayerParent> activePlayers = new List<PlayerParent>();
    private GameUI_Manager uiManager;
    private List<string> players;
    void Start()
    {
        uiManager = GameUI_Manager.Instance;
        uiManager.GameManager = this;
        players = new List<string>();
        
        for(int i = 0; i < 4; i ++)
        {
            players.Add("NPC");
        }
    }
    public void SetPlayer(string _name, int _id)
    {
        if (players[_id] != "NPC")
            return;

        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].Equals(_name))
            {
                photonView.RPC("RPC_SetPlayer", RpcTarget.All, "NPC", i);
            }
        }
        photonView.RPC("RPC_SetPlayer", RpcTarget.All, _name, _id);

        if(!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            uiManager.PlayerMessageText.text = "Waiting For Host to Start Game";
        }
    }
    [PunRPC]
    public void RPC_SetPlayer(string _name, int _id)
    {
        players[_id] = _name;
        uiManager.UpdateTeamSelectUIs();
    }

    public void StartGame()
    {
        photonView.RPC("RPC_StartGame", RpcTarget.All);
    }
    [PunRPC]
    public void RPC_StartGame()
    {
        int _localPlayerNumber = GetPlayerID(PhotonNetwork.LocalPlayer.NickName);

        uiManager.SetGameState(GameState.Running);
        uiManager.MainCamera.SetActive(false);


        GameObject _playerObj =  PhotonNetwork.Instantiate("Player", MyRoom.Instance.SpawnPoints[_localPlayerNumber].position, Quaternion.identity);
        MyPlayer _playerScript = _playerObj.GetComponent<MyPlayer>();
        _playerScript.SetID(_localPlayerNumber);

        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            for(int i = 0; i < 4; i++)
            {
                if(players[i] == "NPC")
                {
                    GameObject _NPC_Obj = PhotonNetwork.Instantiate("NPC", MyRoom.Instance.SpawnPoints[i].position, Quaternion.identity);
                    NPC _NPC_Script = _NPC_Obj.GetComponent<NPC>();
                    _NPC_Script.SetID(i);
                }
            }
        }
    }
    
    public void RemovePlayerThatLeft()
    {
        for(int i = 0; i < 4; i++)
        {
            if(players[i] != "NPC")
            {
                for(int j = 0; j < PhotonNetwork.CurrentRoom.Players.Count; j++)
                {
                    if(players[i] == PhotonNetwork.CurrentRoom.Players[j].NickName)
                    {
                        continue;
                    }

                    SetPlayer("NPC", i);
                }
            }
        }
    }
    public int GetAssignedPlayerCount()
    {
        int _count = 0;
        for(int i = 0; i < 4; i++)
        {
            if (players[i] != "NPC")
                _count++;
        }
        return _count;
    }
    public List<string> GetPlayers()
    {
        return players;
    }
    public int GetPlayerID(string _name)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].Equals(_name))
                return i;
        }
        return 6;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
