using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class EntityBase : MonoBehaviourPun, IPunObservable
{
    protected GameManager gameManager;
    protected string plName;
    public string PlayerName { get => plName; protected set => SetName(value); }
  
    protected int id;
    public int ID { get => id; }

	[SerializeField] protected SpriteRenderer spriteRenderer;
	[SerializeField] protected GameObject gunPoint;
	[SerializeField] protected float shootSpeed;

	protected float health = 100f;
    [SerializeField] private HealthBar healthBar;

    public float Health { get => health; }

    public Team Team { get => id < 2? Team.A : Team.B; }


    [SerializeField] private protected float moveForce = 3;
    [SerializeField] protected ParticleSystem hitParticles;



    private void Awake()
	{
        gameManager = GameUI_Manager.Instance.GameManager;
		gameManager.activeEntities.Add(this);
        GameUI_Manager.Instance.SetGameState(GameState.Running);
	}

	private void OnDestroy()
	{
        gameManager.activeEntities.Remove(this);
	}

	protected void SetName(string value)
    {
        plName = value;
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

    public void DealDamage(int _otherId, float _damage)
    {
        photonView.RPC("RPC_DealDamage", RpcTarget.All,_otherId, _damage);
        photonView.RPC(nameof(RPC_EntityHitParticles), RpcTarget.All);



        if (healthBar != null)
        {
            
            SetPlayerHealth(health);
        } 
    }

    [PunRPC]
    public void RPC_DealDamage(int _otherId,float damage)
    {
        health -= damage;
       
        if (health <= 0 && photonView.IsMine)
        {
            gameManager.EntityDead(ID);

            gameManager.SetKillCount(_otherId);

            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void SetPlayerHealth(float health)
    {
        photonView.RPC("RPC_SetPlayerHealth", RpcTarget.All, health);
    }

    [PunRPC]
    public void RPC_SetPlayerHealth(float health)
    {

        healthBar.SetHealth(health);

    }

    [PunRPC]
    public void RPC_EntityHitParticles()
    {
        hitParticles.Play();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
