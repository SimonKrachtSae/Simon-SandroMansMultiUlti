using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerManager : MonoBehaviourPun, IPunObservable
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
    RoomPlayerSettings settings;
    private List<string> playerNames;


    private void Start()
    {

        settings = new RoomPlayerSettings(PlayerType.NPC,"NPC", PlayerType.NPC, "NPC", PlayerType.NPC, "NPC", PlayerType.NPC, "NPC");
        PhotonNetwork.LocalPlayer.NickName = "Jeff";

        playerNames = new List<string>();
        playerNames.Add(settings.Player1Name);
        playerNames.Add(settings.Player2Name);
        playerNames.Add(settings.Player3Name);
        playerNames.Add(settings.Player4Name);

        playerTypes = new List<PlayerType>();
        playerTypes.Add(settings.Player1);
        playerTypes.Add(settings.Player2);
        playerTypes.Add(settings.Player3);
        playerTypes.Add(settings.Player4);

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
                    if(playerTxts[i].text == PhotonNetwork.LocalPlayer.NickName)
                    {
                        playerTypes[i] = PlayerType.NPC;
                        playerNames[i] = "NPC";
                    }
                }
            }
            playerTypes[id] = PlayerType.Player;
            playerNames[id] = PhotonNetwork.LocalPlayer.NickName;
            OnUIClicked();

        }
        else if(playerTypes[id] == PlayerType.Player)
        {
            if(playerTxts[id].text == PhotonNetwork.LocalPlayer.NickName)
            {
                playerTypes[id] = PlayerType.NPC;
                playerNames[id] = "NPC";
                OnUIClicked();
            }
        }
    }

    
    public void OnUIClicked()
    {
        photonView.RPC("DrawUIs", RpcTarget.All, settings);
    }

    [PunRPC]
    public void DrawUIs(RoomPlayerSettings _settings)
    {
        settings = _settings;
        player1Txt.text = settings.Player1Name;
        player2Txt.text = settings.Player2Name;
        player3Txt.text = settings.Player3Name;
        player4Txt.text = settings.Player4Name;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    
}
public class RoomPlayerSettings
{
    private PlayerType player1;
    public PlayerType Player1 { get => player1; set => player1 = value; }

    private string player1Name;
    public string Player1Name { get => player1Name; set => player1Name = value; }

    private PlayerType player2;
    public PlayerType Player2 { get => player2; set => player2 = value; }

    private string player2Name;
    public string Player2Name { get => player2Name; set => player2Name = value; }

    private PlayerType player3;
    public PlayerType Player3 { get => player3; set => player3 = value; }

    private string player3Name;
    public string Player3Name { get => player3Name; set => player3Name = value; }

    private PlayerType player4;
    public PlayerType Player4 { get => player4; set => player4 = value; }

    private string player4Name;
    public string Player4Name { get => player4Name; set => player4Name = value; }

    public RoomPlayerSettings(PlayerType _player1Type, string _player1Name, PlayerType _player2Type, string _player2Name, PlayerType _player3Type, string _player3Name, PlayerType _player4Type, string _player4Name)
    {
        player1 = _player1Type;
        player1Name = _player1Name;

        player2 = _player2Type;
        player2Name = _player2Name;

        player3 = _player3Type;
        player3Name = Player3Name;

        player4 = _player4Type;
        Player4Name = player4Name;
    }
}
