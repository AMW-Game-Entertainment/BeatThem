using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Score Config")]
    public int requiredScoreToWin;
    public int currentScore;

    public bool isPausedGame;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isPausedGame = !isPausedGame;

        Time.timeScale = isPausedGame ? 0.0f : 1.0f;

        Cursor.lockState = isPausedGame ? CursorLockMode.None : CursorLockMode.Locked;

        GameUI.instance.TogglePauseMenu(isPausedGame);
    }

    public void AddScore(int score)
    {
        currentScore += score;

        GameUI.instance.UpdateScoreText(currentScore);

        if (currentScore >= requiredScoreToWin)
            WinGame();
    }

    void WinGame()
    {
        GameUI.instance.SetEndGameScreen(true, currentScore);

        Time.timeScale = 0.0f;
        isPausedGame = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoseGame()
    {
        GameUI.instance.SetEndGameScreen(false, currentScore);

        Time.timeScale = 0.0f;
        isPausedGame = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
