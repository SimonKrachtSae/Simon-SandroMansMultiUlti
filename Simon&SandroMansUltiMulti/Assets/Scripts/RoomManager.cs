using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    [SerializeField] private List<ServerUI> allRoomUIs;
    List<RoomInfo> availableRooms;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple " + this.name + " detected! \n"
                + "Instance on GameObject: " + gameObject.name + " has been removed.");

            Destroy(this.gameObject);
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        availableRooms = roomList;
        Debug.Log(roomList.Count);
    }
    public void UpdateRoomUIs()
    {
        if (availableRooms.Count > 0)
        {
            for (int i = 0; i < availableRooms.Count; i++)
            {
                allRoomUIs[i].gameObject.SetActive(true);
                allRoomUIs[i].UpdateText(availableRooms[i].PlayerCount);
            }
        }
    }
    public List<RoomInfo> GetRooms()
    {
        return availableRooms;
    }
}
