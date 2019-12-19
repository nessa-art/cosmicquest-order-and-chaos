using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartTutorial()
    {
        // Load Tutorial scene
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }

    public void StartLevel() 
    {
        // Load Level 1
        SceneManager.LoadScene("ChaosVoid1", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}

