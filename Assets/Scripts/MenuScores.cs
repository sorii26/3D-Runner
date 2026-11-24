using UnityEngine;
using TMPro;

public class MenuScores : MonoBehaviour
{
    public TMP_Text bestScoreText;

    void Start()
    {
        bestScoreText.text = "Best Score: " +
            PlayerPrefs.GetInt("BestScore", 0);
    }
}