using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NPC : EntityBase
{
    public bool destroy;
    private void Update()
    {

        if (destroy)
        {
            GameUI_Manager.Instance.GameManager.EntityDead(ID);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

}
