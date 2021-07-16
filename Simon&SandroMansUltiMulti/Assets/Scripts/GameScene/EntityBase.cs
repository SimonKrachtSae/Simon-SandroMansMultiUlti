using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class EntityBase : MonoBehaviourPun, IPunObservable
{
    private GameManager gameManager;
    private string plName;
    public string PlayerName { get => plName; protected set => SetName(value); }
  
    protected int id;
    public int ID { get => id; }

    [SerializeField] protected SpriteRenderer spriteRenderer;
	[SerializeField] protected GameObject gunPoint;
	[SerializeField] protected float ShootSpeed;

	protected float health = 100f;
    [SerializeField] private HealthBar healthBar;

    public float Health { get => health; }

    public Team Team { get => id < 2? Team.A : Team.B; }


    [SerializeField] private protected float moveForce = 3;

    public event System.Action<string> NameChanged;

	private void Awake()
	{
        gameManager = GameUI_Manager.Instance.GameManager;
		gameManager.activePlayers.Add(this);
        GameUI_Manager.Instance.SetGameState(GameState.Running);
	}

	private void OnDestroy()
	{
		gameManager.activePlayers.Remove(this);
	}

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

    public void DealDamage(int _otherId, float _damage)
    {
        photonView.RPC("RPC_DealDamage", RpcTarget.All,_otherId, _damage);
        if (healthBar != null)
        {
            
            SetPlayerHealth(health);
        } 
    }

    [PunRPC]
    public void RPC_DealDamage(int _otherId,float damage)
    {
        health -= damage;
        if (!photonView.IsMine)
        {
            return;
        }

        if (health <= 0)
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


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
