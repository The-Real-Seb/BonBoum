using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGamePanel : MonoBehaviour
{
    public TextMeshProUGUI scoreUI;
    public void ShowEndScore(int playerPoints)
    {
        scoreUI.text = playerPoints.ToString();
    }
}
   
