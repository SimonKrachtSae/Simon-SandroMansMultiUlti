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


    void Start()
    {
        if (!photonView.IsMine)
        {
            playerCam.gameObject.SetActive(false);
        }

    }
    private void Update()
    {
        if(photonView.IsMine)
        {
            float xMove = Input.GetAxisRaw("Horizontal");
            float yMove = Input.GetAxisRaw("Vertical");
            Vector3 moveDir = new Vector3(xMove, yMove, 0).normalized;
            transform.position += moveDir * Time.fixedDeltaTime * moveForce;


            Vector3 mousePos = Input.mousePosition;
            mousePos.z = playerCam.nearClipPlane;
            worldPosition = playerCam.ScreenToWorldPoint(mousePos);
            RotateToward(worldPosition, 4);
            worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0);

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
        }
    }
    public void RotateToward(Vector3 targ, float speed)
    {
        targ.z = 0f;
        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;
        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg - 90;
        playerObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    private void Shoot()
    {
        GameObject _bullet = PhotonNetwork.Instantiate("Bullet", gunPoint.transform.position, Quaternion.identity);
        _bullet.GetComponent<Rigidbody2D>().velocity = (worldPosition - gunPoint.transform.position).normalized * ShootSpeed;
    }
   
}
