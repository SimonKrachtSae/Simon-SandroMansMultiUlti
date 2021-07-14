using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    private GameUI_Manager uiManager;
    private List<string> players;
    private int localPlayerID = 5;
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

        uiManager.SetGameState(GameState.Running);
        uiManager.MainCamera.SetActive(false);


        localPlayerID = GetPlayerID(PhotonNetwork.LocalPlayer.NickName);
        SpawnPlayer();

        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            for(int i = 0; i < 4; i++)
            {
                if(players[i] == "NPC")
                {
                    SpawnNPC(i);
                }
            }
        }
    }
    private void SpawnPlayer()
    {

        GameObject _playerObj = PhotonNetwork.Instantiate("Player", MyRoom.Instance.SpawnPoints[localPlayerID].position, new Quaternion(1, 0, 0, 1));
        PlayerController _playerScript = _playerObj.GetComponent<PlayerController>();
        _playerScript.SetID(localPlayerID);
    }
    private void SpawnNPC(int _id)
    {
        GameObject _NPC_Obj = PhotonNetwork.Instantiate("NPC", MyRoom.Instance.SpawnPoints[_id].position, new Quaternion(1, 0, 0, 1));
        NPC _NPC_Script = _NPC_Obj.GetComponent<NPC>();
        _NPC_Script.SetID(_id);
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
    public void EntityDead(int _id)
    {

        photonView.RPC(nameof(RPC_EntityDead), RpcTarget.All,_id);
    }

    [PunRPC]
    public void RPC_EntityDead(int _id)
    {
        StartCoroutine(YieldRespawn(5, _id));
    }
    private IEnumerator YieldRespawn(float _time, int _id)
    {
        yield return new WaitForSeconds(_time);

        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if(players[_id] == "NPC")
            {
                SpawnNPC(_id);
            }
        }
        if(localPlayerID == _id)
        {
            SpawnPlayer();
        }

        StopCoroutine(YieldRespawn(_time,_id));
    }
}
