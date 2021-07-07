using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ActivePlayer : MonoBehaviourPun, IPunObservable
{
    private string plName;
    public string Name { get => plName; set => plName = value; }

    private int plNumber = 5;
    public int Number { get => plNumber;}
    

    private void Start()
    {
        plNumber = 5;

        if(photonView.IsMine)
        {
            photonView.RPC("SetPlayerName", RpcTarget.All,PhotonNetwork.LocalPlayer.NickName);
            GameManager.Instance.SetAtivePlayer(this);
        }
            GameManager.Instance.Subscribe(this);
    }
    public void SetNumber(int _number)
    {
        photonView.RPC("SetPlayerNumber", RpcTarget.All, _number);
    }
    [PunRPC]
    public void SetPlayerNumber(int _number)
    {
        plNumber = _number;
        GameManager.Instance.UpdateTeamSelectUIs();
    }
    [PunRPC]
    public void SetPlayerName(string _name)
    {
        plName = _name;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
