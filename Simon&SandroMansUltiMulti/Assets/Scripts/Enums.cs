using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public enum Team
{
    A,
    B,
    None
}
public enum ConnectionStatus
{
    Connecting,
    Connected,
    HostingOrJoiningRoom,
    InRoomSelection,
    InRoom
}
public enum PlayerType
{
    Player,
    NPC
}
public enum GameState
{
    TeamSelection,
    Running,
    Paused,
    GameOver
}

public enum NPCstate
{
	Wander,
	Chase,
	Shoot,
	Flee
}
