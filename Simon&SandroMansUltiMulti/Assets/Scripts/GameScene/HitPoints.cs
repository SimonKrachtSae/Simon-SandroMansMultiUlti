using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPoints : MonoBehaviour
{
    [SerializeField] private float hitPoints;
    [SerializeField] private float maxHitPoints = 10f;
    private HealthBar healthBar;

    private void Start()
    {
        hitPoints = maxHitPoints;
        healthBar.SetHealth(hitPoints, maxHitPoints);
    }



    // Dieser Methode unten muss bearbeitet wertden

   /* private void TakeDamage(float damage)
    {
        hitPoints -= damage;
        healthBar.SetHealth(hitPoints, maxHitPoints);
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
        
    } */
}
