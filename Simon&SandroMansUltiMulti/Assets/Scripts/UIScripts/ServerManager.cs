using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class ServerManager : MonoBehaviourPunCallbacks
{
    public static ServerManager Instance;

    [SerializeField] private GameObject roomsPanel;
    [SerializeField] private GameObject singleRoomPanel;

    [SerializeField] private TMP_Text numberText;

    [SerializeField] private List<ServerUI> allServerUIs;

    private List<ServerUI> enabledServerUIs;

    private int selectedServer = 0;

    private int hostedServerID = 0;
    public int SelectedServer
    {
        get => selectedServer;
        set
        {
            Debug.Log(selectedServer);
            selectedServer = value;
        }
    }

    [Header("RoomUI")]
    [SerializeField] private List<TMP_Text> PlayerTexts;

    private void Awake()
    {        
        Debug.Log("cheese");
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple " + this.name + " detected! \n"
                + "Instance on GameObject: " + gameObject.name + " has been removed.");

            Destroy(this.gameObject);
        }
        enabledServerUIs = new List<ServerUI>();
    }
    public void Subscribe(ServerUI _serverUI)
    {
        if (enabledServerUIs.Contains(_serverUI))
            return;

        enabledServerUIs.Add(_serverUI);
        numberText.text = "Servers: " + enabledServerUIs.Count + " / 5";
    }
    public void UnSubscribe (ServerUI _serverUI)
    {
        if (!enabledServerUIs.Contains(_serverUI))
            return;

        enabledServerUIs.Remove(_serverUI);
        numberText.text = "Servers: " + enabledServerUIs.Count + " / 5";
    }
    public void HostServer()
    {
        if (PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.CreateRoom("OfflineMode123");
            roomsPanel.SetActive(false);
            singleRoomPanel.SetActive(true);
            return;
        }

        hostedServerID = GetFreeID();
        if(hostedServerID == 5)
        {
            return;
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom(hostedServerID.ToString(), roomOptions,null);

        roomsPanel.SetActive(false);
        singleRoomPanel.SetActive(true);
    }

    private int GetFreeID()
    {
        for(int i = 0; i < allServerUIs.Count; i++)
        {
            if(!allServerUIs[i].isActiveAndEnabled)
            {
                return allServerUIs[i].ID;
            }
        }
        Debug.Log("All rooms are occupied");
        return 5;
    }
    public void JoinRoom(int id)
    {
        PhotonNetwork.JoinRoom(id.ToString());
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("JoinedRoom");
        roomsPanel.SetActive(false);
        singleRoomPanel.SetActive(true);
        UpdatePlayerTexts();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerTexts();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerTexts();
    }
    private void UpdatePlayerTexts()
    {
        int playerCount = PhotonNetwork.CurrentRoom.Players.Count;

        for (int i = 0; i < PlayerTexts.Count; i++)
        {
            if (i < playerCount)
            {
                PlayerTexts[i].text = ("Player " + (i + 1) + ": " + PhotonNetwork.CurrentRoom.GetPlayer(i +1).NickName);
            }
            else
            {
                PlayerTexts[i].text = ("Player " + (i + 1) + ": NPC ");
            }
        }
    }
    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            SceneManager.LoadScene(1);
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        singleRoomPanel.SetActive(false);
        roomsPanel.SetActive(true);
        RoomManager.Instance.UpdateRoomUIs();
    }
}
