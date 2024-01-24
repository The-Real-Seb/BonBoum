using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

        private static GameManager instance = null;
        public static GameManager Instance => instance;

    #endregion

    public int waveNumber;
    public int playerPoints;

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
}
