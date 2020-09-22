using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public GameObject scoreText;
    public int currentScore;
    public static int generalScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScoreText()
    {
        currentScore += 3 * 5;
        generalScore += currentScore;
        scoreText.GetComponent<TextMeshProUGUI>().text = currentScore.ToString();
    }
}
