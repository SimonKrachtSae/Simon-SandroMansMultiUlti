using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRoom : MonoBehaviour
{
    public static MyRoom Instance;
    [SerializeField] private List<Transform> spawnPoints;
    public List<Transform> SpawnPoints { get => spawnPoints; set => spawnPoints = value; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
