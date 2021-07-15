using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAudioManager : MonoBehaviour
{
    public static NetworkAudioManager Instance;
    [SerializeField] private AudioSource ClickSound;
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
