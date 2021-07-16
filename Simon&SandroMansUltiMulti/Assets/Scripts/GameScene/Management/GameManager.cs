using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun, IPunObservable
{
	public List<EntityBase> activePlayers = new List<EntityBase>();
	private GameUI_Manager uiManager;
    private List<string> players;
    private List<int> killCounts;
    private int localPlayerID = 5;

    void Start()
    {
        uiManager = GameUI_Manager.Instance;
        uiManager.GameManager = this;
        players = new List<string>();
        killCounts = new List<int>();
        
        for(int i = 0; i < 4; i ++)
        {
            players.Add("NPC");
            killCounts.Add(0);
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
        GameObject _playerObj = PhotonNetwork.Instantiate("Player", MyRoom.Instance.SpawnPoints[localPlayerID].position, Quaternion.identity);
        PlayerController _playerScript = _playerObj.GetComponent<PlayerController>();
        _playerScript.SetID(localPlayerID);
    }
    private void SpawnNPC(int _id)
    {
        GameObject _NPC_Obj = PhotonNetwork.Instantiate("NPC", MyRoom.Instance.SpawnPoints[_id].position, Quaternion.identity);
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
        if (GameUI_Manager.Instance.GetGameState() != GameState.Running)
            return;
        uiManager.SetGameState(GameState.Respawning);

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
    public void SetKillCount(int _id)
    {
        photonView.RPC(nameof(RPC_SetKillCount), RpcTarget.All, _id);
    }
    [PunRPC]
    public void RPC_SetKillCount(int _id)
    {
        killCounts[_id]++;
    }
    public void GameOver()
    {
        int teamAKills = killCounts[0] + killCounts[1];
        int teamBKills = killCounts[2] + killCounts[3];
        string winnerTeam;

        if(teamAKills > teamBKills)
        {
            winnerTeam = "Blue";
        }
        else if(teamAKills == teamBKills)
        {

            winnerTeam = "Tie";
        }
        else
        {
            winnerTeam = "Red";

        }
        photonView.RPC(nameof(RPC_GameOver), RpcTarget.All,  winnerTeam);

        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            return;
      
        for(int i = 0; i < activePlayers.Count; i++)
        {
             PhotonNetwork.Destroy(activePlayers[i].gameObject);
        }
    }
    [PunRPC]
    public void RPC_GameOver(string _winTeam)
    {
        GameUI_Manager.Instance.SetGameState(GameState.GameOver);
        GameUI_Manager.Instance.SetGameOverTexts(players, killCounts);
        GameUI_Manager.Instance.WinnerText.text = "Winner" + _winTeam;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
