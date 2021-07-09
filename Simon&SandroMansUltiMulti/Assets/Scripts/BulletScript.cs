using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviourPun, IPunObservable
{
    private MyPlayer localPlayer;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MyPlayer player = collision.gameObject.GetComponent<MyPlayer>();
            if (player.GetTeam()!= localPlayer.GetTeam())
            {
                player.Damage(20);
            }
            Destroy(this.gameObject);
        }
    }
    
    public void SetLocalPlayer(MyPlayer player)
    {
        localPlayer = player;
    }
}
