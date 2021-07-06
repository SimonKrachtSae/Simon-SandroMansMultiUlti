using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerManager : MonoBehaviourPun
{
    //TeamA
    private PlayerType Player1;
    [SerializeField] private TMP_Text player1Txt;

    private PlayerType Player2;
    [SerializeField] private TMP_Text player2Txt;

    //TeamB
    private PlayerType Player3;
    [SerializeField] private TMP_Text player3Txt;

    private PlayerType Player4;
    [SerializeField] private TMP_Text player4Txt;

    List<TMP_Text> playerTxts;
    List<PlayerType> playerTypes;

    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = "Jeff";
        Player1 = PlayerType.NPC;
        Player2 = PlayerType.NPC;
        Player3 = PlayerType.NPC;
        Player4 = PlayerType.NPC;
        playerTypes = new List<PlayerType>();

        playerTypes.Add(Player1);
        playerTypes.Add(Player2);
        playerTypes.Add(Player3);
        playerTypes.Add(Player4);

        playerTxts = new List<TMP_Text>();
        playerTxts.Add(player1Txt);
        playerTxts.Add(player2Txt);
        playerTxts.Add(player3Txt);
        playerTxts.Add(player4Txt);
    }

    public void SetPlayerType(int id)
    {

        if(playerTypes[id] == PlayerType.NPC)
        {
            for (int i = 0; i < 4; i++)
            {
                if(i != id)
                {
                    if(playerTxts[id].text == PhotonNetwork.LocalPlayer.NickName)
                    {
                        playerTypes[id] = PlayerType.NPC;
                        playerTxts[id].text = "NPC";
                    }
                }
            }
            playerTypes[id] = PlayerType.Player;
            playerTxts[id].text = PhotonNetwork.LocalPlayer.NickName; 

        }
        else if(playerTypes[id] == PlayerType.Player)
        {
            if(playerTxts[id].text == PhotonNetwork.LocalPlayer.NickName)
            {
                playerTypes[id] = PlayerType.NPC;
                playerTxts[id].text = "NPC";
            }
        }
    }

}
