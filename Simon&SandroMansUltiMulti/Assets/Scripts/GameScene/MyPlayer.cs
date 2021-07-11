using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MyPlayer : PlayerParent
{   
    void Start()
    {
        if (!photonView.IsMine)
        {
            playerCam.SetActive(false);
        }

    }
}
