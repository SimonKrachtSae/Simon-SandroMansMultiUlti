using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ServerManager : MonoBehaviourPunCallbacks
{
    public static ServerManager Instance;

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
   
    private void Awake()
    {
        Debug.Log("cheese");
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
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
        _serverUI.UpdateText();
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

        if(hostedServerID > 0)
        {
            Debug.Log("Already Hosting Server!");
            return;
        }

        if (enabledServerUIs.Count == 5)
            return;

        hostedServerID = GetFreeServerID();

        if (hostedServerID == 0)
            return;

        allServerUIs[hostedServerID - 1].gameObject.SetActive(true);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom(hostedServerID.ToString(), roomOptions);

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("FIIIIIIIIIIIRE");
    }
    public override void OnCreatedRoom()
    {
        allServerUIs[hostedServerID - 1].PlayerCount++;
    }
    
    private int GetFreeServerID()
    {
        if(enabledServerUIs.Count == 0)
        {
            return 1;
        }

        for (int i = 0; i < allServerUIs.Count; i ++)
        {
            if(!allServerUIs[i].isActiveAndEnabled)
            {
                return allServerUIs[i].ID;
            }
        }
        return 0;
    }
    private ServerUI GetServer(int _ID)
    {
        for (int i = 0; i < allServerUIs.Count; i++)
        {
            if (allServerUIs[i].ID == _ID)
            {
                return allServerUIs[i];
            }
        }
        return null;
    }
}
