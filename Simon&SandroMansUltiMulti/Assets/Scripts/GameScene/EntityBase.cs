using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class EntityBase : MonoBehaviourPun
{
    private string plName;
    public string PlayerName { get => plName; protected set => SetName(value); }
  
    protected int id;
    public int ID { get => id; }

    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected float health = 100f;

    public float Health { get => health; }

    public Team Team { get => id < 2? Team.A : Team.B; }


    [SerializeField] private protected float moveForce = 3;

    public event System.Action<string> NameChanged;


    protected void SetName(string value)
    {
        plName = value;
        NameChanged?.Invoke(value);
    }

    public void SetID(int _id)
    {
       

        if (_id < 2)
        {
            photonView.RPC(nameof(RPC_SetPlayerID), RpcTarget.All, _id);
        }
        else
        {
            photonView.RPC(nameof(RPC_SetPlayerID), RpcTarget.All, _id);
        }
    }
    [PunRPC]
    public void RPC_SetPlayerID(int _id)
    {
        id = _id;

        if(Team == Team.A)
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

        if (!photonView.IsMine)
            return;

        if (health <= 0)
        {
            GameUI_Manager.Instance.GameManager.EntityDead(ID);

            //GameUI_Manager.Instance.MainCamera.SetActive(true);

            PhotonNetwork.Destroy(this.gameObject);
        }
       
    }
}
