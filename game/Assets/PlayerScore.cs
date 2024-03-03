using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int score = 0;
    public TextMeshProUGUI scoreValue;
    public void IncreaseScore(int points)
    {
        score += points;
        scoreValue.SetText(score.ToString());
        Debug.Log("Score: " + score);

    }
}
