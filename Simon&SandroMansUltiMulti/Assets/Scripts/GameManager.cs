using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private ActivePlayer activePlayer;
    
    private List<ActivePlayer> activePlayers;
    [SerializeField] private List<TMP_Text> playerTxts;

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
        activePlayers = new List<ActivePlayer>();
    }
    private void Start()
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

        for (int i = 0; i < 4; i++)
        {
            playerTxts[i].text = "NPC";
        }
    }
    public void SetPlayerNumber(int number)
    {
        if (playerTxts[number].text == PhotonNetwork.LocalPlayer.NickName)
            return;

        if (playerTxts[number].text == "NPC")
        {
            playerTxts[number].text = PhotonNetwork.LocalPlayer.NickName;
            this.activePlayer.SetNumber(number);
        }
    }
    public void Subscribe(ActivePlayer activePlayer)
    {
        activePlayers.Add(activePlayer);
    }
    private void Update()
    {

        for (int i = 0; i < 4; i++)
        {
            if(playerTxts[i].text == "NPC")
            {
                for(int j = 0; j < activePlayers.Count; j++)
                {
                    if (i == activePlayers[j].Number)
                    {
                        playerTxts[i].text = activePlayers[j].Name;
                    }
                }
            }
        }
        //for(int i = 0; i < 4; i++)
        //{
        //    if(playerTxts[i].text != "NPC")
        //    {
        //        for(int j = 0; j < activePlayers.Count; j++)
        //        {
        //            if(playerTxts[i].text == activePlayers[j].Name)
        //            {
        //                if(activePlayers[j].Number != i)
        //                {
        //                    playerTxts[i].text = "NPC";
        //                }
        //            }
        //        }
        //    }
        //}
    }
    public void SetAtivePlayer(ActivePlayer _activePlayer)
    {
        activePlayer = _activePlayer;
    }
}
