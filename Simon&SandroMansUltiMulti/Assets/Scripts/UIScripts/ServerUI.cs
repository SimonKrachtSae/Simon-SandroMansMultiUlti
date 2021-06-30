using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ServerUI : MonoBehaviour
{

    private int playerCount = 0;
    public int PlayerCount
    {
        get => playerCount;
        set
        {
            playerCount = value;
            UpdateText();
        }
    }
    [SerializeField] private TMP_Text text;


    [SerializeField] private int id;
    public int ID { get => id; }
    public void UpdateText()
    {
        text.text = "Server: " + ID + " / Players: " +  playerCount;
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
}
