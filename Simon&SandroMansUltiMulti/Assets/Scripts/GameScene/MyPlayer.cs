using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MyPlayer : PlayerParent
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject gunPoint;
    [SerializeField] private float ShootSpeed;
    public Vector3 worldPosition;
    public Vector3 pointPos;
    void Start()
    {
        if (!photonView.IsMine)
        {
            playerCam.gameObject.SetActive(false);
        }

    }
    private void Update()
    {
        pointPos = gunPoint.transform.position;
        if(photonView.IsMine)
        {
            float xMove = Input.GetAxisRaw("Horizontal");
            float zMove = Input.GetAxisRaw("Vertical");
            Vector3 moveDir = new Vector3(xMove, 0, zMove).normalized;
            //transform.position += moveDir * Time.fixedDeltaTime * moveForce;


            Vector3 mousePos = Input.mousePosition;
            mousePos.z = playerCam.nearClipPlane;
            worldPosition = playerCam.ScreenToWorldPoint(mousePos);
            RotateToward(worldPosition, 4);
            Debug.DrawLine(transform.position, worldPosition, Color.red);

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
        }
    }
    public void RotateToward(Vector3 targ, float speed)
    {
        targ.y = 0f;
        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.z = targ.z - objectPos.z;
        float angle = Mathf.Atan2(targ.z, targ.x) * Mathf.Rad2Deg - 90;
        playerObj.transform.rotation = Quaternion.Euler(new Vector3(90,-angle, 0));
    }
    private void Shoot()
    {
        GameObject _bullet = PhotonNetwork.Instantiate("Bullet", gunPoint.transform.position, new Quaternion(1,0,0,1));
        float xDir = worldPosition.x - gunPoint.transform.position.x;
        float zDir = worldPosition.z - gunPoint.transform.position.z;
        
        _bullet.GetComponent<Rigidbody>().velocity = (new Vector3(xDir,0,zDir)).normalized * ShootSpeed;
    }
   
}
