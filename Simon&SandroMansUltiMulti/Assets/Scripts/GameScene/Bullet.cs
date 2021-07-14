using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun, IPunObservable
{
    private int entityID;
    private float damage;


    public void SetPlayer(int _entityID)
    {
        entityID = _entityID;
        //punRPC comunicate onwer ID
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EntityBase _player))
        {
            //if (_player.Team != player.Team)
            //{
            //    _player.DealDamage(20);
            //}
        }
        Destroy(this.gameObject);

    }




























    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
