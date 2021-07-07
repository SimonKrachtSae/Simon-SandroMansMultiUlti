using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerManager : MonoBehaviourPun, IPunObservable
{
    //TeamA
    private PlayerType Player1;

    private PlayerType Player2;

    //TeamB
    private PlayerType Player3;

    private PlayerType Player4;

    [SerializeField] private List<TMP_Text> playerTxts;
    List<PlayerType> playerTypes;


    private void Start()
    {

        playerTypes = new List<PlayerType>();
        playerTypes.Add(Player1);
        playerTypes.Add(Player2);
        playerTypes.Add(Player3);
        playerTypes.Add(Player4);

        for(int i = 0; i < playerTypes.Count; i++)
        {
            playerTypes[i] = PlayerType.NPC;
            playerTxts[i].text = "NPC";
        }
    }

    

    public void SetPlayerType(int id)
    {

        if(playerTxts[id].text == "NPC")
        {
            for (int i = 0; i < 4; i++)
            {
                if(i != id)
                {
                    if(playerTxts[i].text == PhotonNetwork.LocalPlayer.NickName)
                    {
                        playerTxts[i].text = "NPC";
                    }
                }
            }
            playerTxts[id].text = PhotonNetwork.LocalPlayer.NickName;


        }
        else if(playerTxts[id].text != "NPC")
        {
            if(playerTxts[id].text == PhotonNetwork.LocalPlayer.NickName)
            {
                playerTxts[id].text = "NPC";
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(playerTxts[0].text);
            stream.SendNext(playerTxts[1].text);
            stream.SendNext(playerTxts[2].text);
            stream.SendNext(playerTxts[3].text);
        }
        else
        {
            playerTxts[0].text = (string)stream.ReceiveNext();
            playerTxts[1].text = (string)stream.ReceiveNext();
            playerTxts[2].text = (string)stream.ReceiveNext();
            playerTxts[3].text = (string)stream.ReceiveNext();
        }
    }

    
}


