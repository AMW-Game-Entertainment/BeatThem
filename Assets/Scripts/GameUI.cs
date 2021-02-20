using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ammoText;
    public Image healthBarFill;

    [Header("Pause Menu")]
    public GameObject pauseMenu;

    [Header("End Game Screen")]
    public GameObject endGameScreen;
    public TextMeshProUGUI endGameHeaderText;
    public TextMeshProUGUI endGameScoreText;

    public static GameUI instance;

    private void Awake()
    {
        // Create our singleton
        instance = this;
    }

    public void UpdateHealthBar(int curHp, int maxHp)
    {
        healthBarFill.fillAmount = (float)curHp / (float)maxHp;
    }

    public void UpdateScoreText(int amount)
    {
        scoreText.text = "Score: " + amount;
    }

    public void UpdateAmmoText(int curAmmo, int maxAmmo)
    {
        ammoText.text = "Ammo: " + curAmmo + " / " + maxAmmo;
    }

    public void TogglePauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
    }

    public void SetEndGameScreen (bool hasWon, int score)
    {
        endGameScreen.SetActive(true);
        endGameHeaderText.text = hasWon ? "You saved your country" : "You lost your country";
        endGameHeaderText.color = hasWon ? Color.green : Color.red;
        endGameScoreText.text = "<b>Score</b>\n " + score;
    }

    public void OnResumeButton()
    {
        GameManager.instance.TogglePauseGame();
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
