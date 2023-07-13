using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

     
public class SettingsControls : MonoBehaviour
{

    //public int q = GetComponent<Dropdown>().value;
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;

public Dropdown Graphics;


    public void FullScreenToggle()
    {
        bool isFullScreen = !Screen.fullScreen;
        Debug.Log("Fulscreen toggled");
    }
    
    public void Quality()
    {
        int q = Graphics.value;
        QualitySettings.SetQualityLevel(q);
        Debug.Log("Selected quality level: " + q);

    }

    public void ShowMainMenu()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

}
