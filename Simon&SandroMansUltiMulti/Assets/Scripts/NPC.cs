using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NPC : MonoBehaviourPun, IPunObservable
{
    private int id;
    public int ID { get => id; set => id = value; }    private SpriteRenderer spriteRenderer;
    private Team team;
    
    void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetTeam(Team _team)
    {
        photonView.RPC("RPC_SetTeam", RpcTarget.All, _team);
    }
    [PunRPC]
    public void RPC_SetTeam(Team _team)
    {
        team = _team;
        if (team == Team.A)
        {
            spriteRenderer.color = Color.blue;
        }
        else if (team == Team.B)
        {
            spriteRenderer.color = Color.red;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
