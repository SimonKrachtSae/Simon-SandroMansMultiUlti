using UnityEngine;
using TMPro;
using UnityEditor;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
public enum RoomProperties
{
    RoomName
}
public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject roomPanel;

    [Header("UI Elements Menu")]
    [SerializeField] private TMP_InputField nameText;
    [SerializeField] private GameObject missingNameText;

    [Header("UI Elements Room")]
    [SerializeField] private TMP_Text playerCount;
    [SerializeField] private GameObject startGameButton;

    [Header("RoomSettings")]
    [SerializeField] private int roomSize = 4;
    [SerializeField] private bool isVisible = true;

    Hashtable roomInformations;
    private void Awake()
    {
        mainMenuPanel.SetActive(false);
        roomPanel.SetActive(false);

        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting");
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        roomPanel.SetActive(false);
        missingNameText.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        mainMenuPanel.SetActive(true);
        Debug.Log("We Are Connected to a Master Server");

    }
    public void JoinLobby() 
    {
        if (string.IsNullOrWhiteSpace(nameText.text))
        {
            missingNameText.SetActive(true);
            return;
        }

        roomPanel.SetActive(true);
        mainMenuPanel.SetActive(false);

        PhotonNetwork.JoinRandomRoom();

        PhotonNetwork.LocalPlayer.NickName = nameText.text;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Couldn't locate opened Room");
        Debug.Log("Lets create one");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSize;
        roomOptions.IsVisible = isVisible;

        PhotonNetwork.CreateRoom(string.Empty,roomOptions);
    }

    

    public override void OnCreatedRoom()
    {
        //roomInformations = new Hashtable();
        //roomInformations.Add(RoomProperties.RoomName, "FirstRoom");
        //PhotonNetwork.CurrentRoom.SetCustomProperties(roomInformations);
    }

    public override void OnJoinedRoom()
    {
        roomPanel.SetActive(true);
        playerCount.text = PhotonNetwork.CurrentRoom.Players.Count.ToString();

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

        Debug.Log("Joined Room!");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerCount.text = PhotonNetwork.CurrentRoom.Players.Count.ToString();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerCount.text = PhotonNetwork.CurrentRoom.Players.Count.ToString();
    }
    public override void OnLeftRoom()
    {
        mainMenuPanel.SetActive(true);
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        
    }
}
