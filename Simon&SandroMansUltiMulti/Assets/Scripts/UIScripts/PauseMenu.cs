using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuUI;
    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            PauseMenuUI.SetActive(true);
        }
    }
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
    }
    public void QuitGame()
    {

    }
}
