using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MyPlayer : MonoBehaviourPun, IPunObservable
{
    private string plName;
    public string Name { get => plName; set => plName = value; }
    private int id;
    public int ID { get => id; set => id = value; }
    [SerializeField] private GameObject playerCam;
    private SpriteRenderer spriteRenderer;

    private Team team;
    

    void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        if (!photonView.IsMine)
        {
            playerCam.SetActive(false);
        }

    }
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
    public void SetID(int _id)
    {
        if(_id < 2)
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
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
