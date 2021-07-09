using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class MyUI_Handler : MonoBehaviour
{
    public static MyUI_Handler Instance;
    private MyGameManager gameManager;
    public MyGameManager GameManager { get => gameManager; set => gameManager = value; }
    [SerializeField] private List<TMP_Text> playerButtonTexts;
    public List<TMP_Text> PlayerButtontexts { get => playerButtonTexts; set => playerButtonTexts = value; }

    [SerializeField] private GameObject startButton;
    public GameObject StartButton { get => startButton; set => startButton = value; }

    [SerializeField] private GameObject teamSelectPanel;
    public GameObject TeamSelectPanel { get => teamSelectPanel; set => teamSelectPanel = value; }
    [SerializeField] private GameObject room;
    [SerializeField] private GameObject mainCam;

    
    
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
    }
    public void SetPlayerText(int number)
    {
        string localName = PhotonNetwork.LocalPlayer.NickName;
        if(playerButtonTexts[number].text == "NPC")
        {
            for(int i = 0; i < 4; i++)
            {
                if (playerButtonTexts[i].text == localName)
                {
                    playerButtonTexts[i].text = "NPC";
                }
            }
            playerButtonTexts[number].text = localName;
            gameManager.UpdatePlayerText(number, localName);
        }
    }
    public void StartGame()
    {
        int readyPlayers = 0;
        for(int i = 0; i < playerButtonTexts.Count; i++)
        {
            if(playerButtonTexts[i].text != "NPC")
            {
                readyPlayers++;
            }
        }

        if (readyPlayers == PhotonNetwork.CurrentRoom.Players.Count)
        {
            gameManager.StartGame();
        }
    }
    public void UpdatePanelsOnStart()
    {
        teamSelectPanel.SetActive(false);
        mainCam.SetActive(false);
        room.SetActive(true);
        SpawnPlayers();
    }
    private void SpawnPlayers()
    {
        GameObject player = PhotonNetwork.Instantiate("Player", MyRoom.Instance.SpawnPoints[gameManager.LocalPlayerNumber].position, Quaternion.identity);
        MyPlayer playerScript = player.GetComponent<MyPlayer>();

        if(gameManager.LocalPlayerNumber < 2)
        {
            playerScript.SetTeam(Team.A);
        }
        else
        {
            playerScript.SetTeam(Team.B);
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            for (int i = 0; i < 4; i++)
            {
                if (playerButtonTexts[i].text == "NPC")
                {
                    GameObject NPCobj = PhotonNetwork.Instantiate("NPC", MyRoom.Instance.SpawnPoints[i].position, Quaternion.identity);
                    NPC npcScript = NPCobj.GetComponent<NPC>();
                    if(i < 2)
                    {
                        npcScript.SetTeam(Team.A);
                    }
                    else
                    {
                        npcScript.SetTeam(Team.B);
                    }
                }
            }
        }
    }
}
