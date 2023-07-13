using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;

    public void StartPressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
}
