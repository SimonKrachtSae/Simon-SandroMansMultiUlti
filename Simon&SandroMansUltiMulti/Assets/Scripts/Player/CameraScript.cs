using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private float smooth = 0.3f;
    [SerializeField] private float CamHeight;

    private Vector3 velocity = Vector3.zero;




     private void Update()
     {
        Vector3 pos = new Vector3();
        pos.x = Player.position.x;
        pos.z = Player.position.z - 5f;
        pos.y = Player.position.y + CamHeight;
        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smooth);
     }


}
