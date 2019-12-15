using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartTutorial()
    {
        // TODO: In Build settings, ensure Tutorial.unity is indexed to 1
        Debug.Log("Set Tutorial.unity to index 1 in build settings");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartLevel1() 
    {
        // TODO: In Build settings, ensure ChaosVoid1.unity is indexed to 2
        Debug.Log("Set Tutorial.unity to index 2 in build settings");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}

