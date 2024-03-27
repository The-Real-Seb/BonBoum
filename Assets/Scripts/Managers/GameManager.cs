using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

        private static GameManager instance = null;
        public static GameManager Instance => instance;

    #endregion

    public int waveNumber;
    public int playerPoints;
    public bool isGamePaused;
    public bool isPlayerDead;

    public GameObject settingsPanel;
    public EndGamePanel endGamePanel;

    public void PauseGame()
    {
        if(isPlayerDead) { return; }

        if (isGamePaused)
        {
            settingsPanel.SetActive(false);
            Cursor.visible = false;
            Screen.lockCursor = true;
            isGamePaused = false;
        }
        else
        {
            settingsPanel.SetActive(true);
            Cursor.visible = true;
            Screen.lockCursor = false;
            isGamePaused = true;
        }
    }

    public void ShowEndScore()
    {
        endGamePanel.gameObject.SetActive(true);

        Cursor.visible = true;
        Screen.lockCursor = false;
        isGamePaused = true;

        endGamePanel.ShowEndScore(playerPoints);
        isPlayerDead = true;
    }

    public void AddPlayerPoints(int points)
    {
        playerPoints += points;
    }

    public void MinusPlayerPoints(int points)
    {
        if (playerPoints >= points)
        {
            playerPoints -= points;
        }
    }

    public void AddWave()
    {
        waveNumber++;
        WaveManager.Instance.NewWave();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    public void Reset()
    {
        Cursor.visible = false;
        Screen.lockCursor = true;
        isGamePaused = false;        
        isPlayerDead = false;

        SceneManager.LoadScene(0);
    }
}
