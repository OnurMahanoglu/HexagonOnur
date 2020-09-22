using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject PausePanel;

    private void Start()
    {
        PausePanel.SetActive(false);
    }
    public void PauseTheGame()
    {
        PausePanel.SetActive(true);
        SwipeController.isPaused = true;
        Time.timeScale = 0;
    }
    public void ResumeTheGame()
    {
        PausePanel.SetActive(false);
        SwipeController.isPaused = false;
        Time.timeScale = 1;
    }
    public void QuitTheGame()
    {
        Application.Quit();
    }
}
