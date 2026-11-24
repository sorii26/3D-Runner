using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;

    public TMP_Text bestScoreText;
    public Slider volumeSlider;

    void Start()
    {
        Time.timeScale = 1f;

        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);

        // Display saved best score
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        bestScoreText.text = "Best Score: " + bestScore;

        // Load saved volume
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        volumeSlider.value = volume;
        AudioListener.volume = volume;
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("GameScene");
    }

    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public void QuitGame()
    {
        Debug.Log("Game Closed");

        Application.Quit();
    }
}