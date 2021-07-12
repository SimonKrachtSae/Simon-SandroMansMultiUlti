using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private GameObject BulletSpawnPoint;
    [SerializeField] private GameObject BulletPrefab;

    
    [SerializeField] private float speed = 10f;
    [SerializeField] private float timeBetweenShots;




    private void Update()
    {
        // if (photonView.isMine)
        // {
        // 
        //    playerCam.SetActive(false);
        //    hier kommt noch der Code von Movement und Bulletspawn
        //  }
        MousePos();
        Move();
        Shoot();
    }

   private void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        // OR


      //  moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"),0f);
       //  moveVelocity = moveInput * Speed;
    }

    private void MousePos()
    {
        // Tracking the cam over the plane
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        
        Ray camRay = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDistance = 0.0f;

        if (groundPlane.Raycast(camRay, out hitDistance))
        {
            Vector3 targetPoint = camRay.GetPoint(hitDistance);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            PlayerObject.transform.rotation = Quaternion.Slerp(PlayerObject.transform.rotation, targetRotation, 10f * Time.deltaTime);

        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(BulletPrefab.transform, BulletSpawnPoint.transform.position, BulletSpawnPoint.transform.rotation);
        }
        

       /*  if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = PhotonNetwork.Instantiate("BulletPrefab", transform.position + transform.up * 2f, Quaternion.identity);
            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            bulletScript.SetLocalPlayer(this);
            bullet.GetComponent<Rigidbody2D>().velocity = transform.up * Time.fixedDeltaTime * 100f;
        } */

    }
    
}
