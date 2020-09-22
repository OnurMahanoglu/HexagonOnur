using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    public GameObject GameOverPanel;

    private void Start()
    {
        GameOverPanel.SetActive(false);
    }
    public void RestartGame()
    {
        GameOverPanel.SetActive(true);
        SwipeController.isPaused = true;
        Time.timeScale = 0;
        SceneManager.LoadScene("SampleScene");
        
    }
}
