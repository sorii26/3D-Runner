using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerController player;

    public TMP_Text scoreText;
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    public TMP_Text bestScoreText;

    private float score = 0f;
    private int bestScore;
    private bool gameEnded = false;

    void Start()
    {
        Time.timeScale = 1f;

        if (player == null)
            player = FindAnyObjectByType<PlayerController>();

        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        gameOverPanel.SetActive(false);
        scoreText.gameObject.SetActive(true);

        UpdateScore();
    }

    void Update()
    {
        if (!gameEnded && player != null)
        {
            // Increase score while running
            score += player.forwardSpeed * Time.deltaTime;

            UpdateScore();
        }
    }

    public void AddScore(int amount)
    {
        // Add points from collecting cash
        if (!gameEnded)
        {
            score += amount;
            UpdateScore();
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + Mathf.FloorToInt(score);
    }

    public void GameOver()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        Time.timeScale = 0f;

        int finalScore = Mathf.FloorToInt(score);

        // Update and save the best score
        bestScore = Mathf.Max(bestScore, finalScore);

        PlayerPrefs.SetInt("BestScore", bestScore);
        PlayerPrefs.Save();

        // Display results
        finalScoreText.text = "Score: " + finalScore;
        bestScoreText.text = "Best Score: " + bestScore;

        scoreText.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}