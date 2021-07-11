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

    [SerializeField] private protected GameObject playerCam;
    private protected SpriteRenderer spriteRenderer;

    private protected Team team;
    private protected void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }
    public void SetID(int _id)
    {
        if (_id < 2)
        {
            photonView.RPC("RPC_SetID", RpcTarget.All, _id, Team.A, Color.blue);
        }
        else
        {
            photonView.RPC("RPC_SetID", RpcTarget.All, _id, Team.B, Color.red);
        }
    }
    [PunRPC]
    public void RPC_SetID(int _id, Team _team, Color _color)
    {
        id = _id;
        team = _team;
        spriteRenderer.color = _color;
    }
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
