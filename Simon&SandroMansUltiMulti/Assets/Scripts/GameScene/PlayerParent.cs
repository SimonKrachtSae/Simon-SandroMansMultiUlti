using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerParent : MonoBehaviourPun, IPunObservable
{
    private protected string plName;
    public string Name { get => plName; set => plName = value; }

    private protected int id;
    public int ID { get => id; set => id = value; }

    [SerializeField] private protected SpriteRenderer spriteRenderer;

    private protected float health = 100f;
    public Team Team { get => team; set => team = value; }


    private protected Team team;
    [SerializeField] private protected float moveForce = 3;
    public void SetID(int _id)
    {
        if (_id < 2)
        {
            photonView.RPC("RPC_SetID", RpcTarget.All, _id, Team.A);
        }
        else
        {
            photonView.RPC("RPC_SetID", RpcTarget.All, _id, Team.B);
        }
    }
    [PunRPC]
    public void RPC_SetID(int _id, Team _team)
    {
        id = _id;
        team = _team;

        if(_team == Team.A)
        {

            spriteRenderer.color = Color.blue;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    public void DealDamage(float damage)
    {
        photonView.RPC("RPC_DealDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    public void RPC_DealDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
           
        }
       
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
