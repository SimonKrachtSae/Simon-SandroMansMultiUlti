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

    
    private float health = 100f;
   
    
    
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
        else
        {
            MyUI_Handler.Instance.GameManager.LocalPlayer = this;
        }
        MyUI_Handler.Instance.GameManager.Subscribe(this);

          
    }
    void Update()
    {
        if (photonView.IsMine)
        {
        
            if (Input.GetMouseButtonDown(0))
            {
                GameObject bullet = PhotonNetwork.Instantiate("BulletPrefab", transform.position + transform.up * 2f, Quaternion.identity);
                BulletScript bulletScript = bullet.GetComponent<BulletScript>();
                bulletScript.SetLocalPlayer(this);
                bullet.GetComponent<Rigidbody2D>().velocity = transform.up * Time.fixedDeltaTime * 100f;       
            }

        }
    }
   
   
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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

    public Team GetTeam()
    {
        return team;
    }

    public void Damage(float value)
    {
        photonView.RPC("RPC_Damage", RpcTarget.All, value);
    }

   [PunRPC]
   public void RPC_Damage(float value)
    {
        health -= value;
        if (health < 0)
        {
            Destroy(this.gameObject);
        }
    }


}
