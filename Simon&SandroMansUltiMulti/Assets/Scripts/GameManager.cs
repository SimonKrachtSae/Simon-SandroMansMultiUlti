using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(2, 1, 2), Quaternion.identity);
    }
}
