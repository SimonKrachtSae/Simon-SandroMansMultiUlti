using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class HealthBar : MonoBehaviourPun
{

    [SerializeField] private Slider Slider;
    [SerializeField] private Color low;
    [SerializeField] private Color High;
    [SerializeField] private Vector3 Offset;
   

    public void SetHealth(float health, float maxHealth)
    {
        Slider.gameObject.SetActive(health < maxHealth);
        Slider.value = health;
        Slider.maxValue = maxHealth;

        Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, High, Slider.normalizedValue);

    }


    private void Update()
    {
        //Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }


    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }*/
}
