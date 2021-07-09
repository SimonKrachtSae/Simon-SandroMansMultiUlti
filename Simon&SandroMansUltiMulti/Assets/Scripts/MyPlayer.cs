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


    void Start()
    {
        if (!photonView.IsMine)
        {
            playerCam.SetActive(false);
        }
        else
        {
            MyUI_Handler.Instance.GameManager.LocalPlayer = this;
        }
        MyUI_Handler.Instance.GameManager.Subscribe(this);
        
    }
    void Update()
    {
        
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
