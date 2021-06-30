using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ActivePlayer : MonoBehaviourPun, IPunObservable
{
    private float health;

    public float Health
    {
        set
        {
            health = value;

        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
    }
    public void OnHit(float damage)
    {
        photonView.RPC("OnHitRPC", RpcTarget.All, damage);
    }

    [PunRPC]
    public void OnHitRPC(float damage)
    {
        health -= damage;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (float)stream.ReceiveNext();
        }
    }
}
