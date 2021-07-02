using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
public class ServerUI : MonoBehaviour
{

    private int playerCount = 0;
    public int PlayerCount
    {
        get => playerCount;
        set
        {
            playerCount = value;
            //UpdateText();
        }
    }
    [SerializeField] private TMP_Text text;


    [SerializeField] private int id;
    public int ID { get => id; }
    public void UpdateText(int _playerCount)
    {
        text.text = "Server: " + ID + " / Players: " +  _playerCount;
    }
    

    private void OnEnable()
    {
        ServerManager.Instance.Subscribe(this);
    }
    private void OnDisable()
    {
        ServerManager.Instance.UnSubscribe(this);
    }
    public void SetSelectedServer()
    {
        ServerManager.Instance.SelectedServer = ID;
    }
    public void JoinServer(int id)
    {
        PhotonNetwork.JoinRoom(id.ToString());
    }
    public void JoinRoom()
    {
        ServerManager.Instance.JoinRoom(id);
    }
}
