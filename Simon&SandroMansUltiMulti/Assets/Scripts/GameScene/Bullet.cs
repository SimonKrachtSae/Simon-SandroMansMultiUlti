using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun, IPunObservable
{
    private PlayerParent player;
    private float damage;




    public void SetPlayer(PlayerParent _player)
    {
        player = _player;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerParent _player = collision.gameObject.GetComponent<PlayerParent>();

            if (_player.Team != player.Team)
            {
                _player.DealDamage(20);
            }
            Destroy(this.gameObject);
        }
        else
        {
         Destroy(this.gameObject);
        }
      
    }




























    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
