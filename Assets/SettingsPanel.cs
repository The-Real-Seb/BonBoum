using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Slider xSens;
    public Slider ySens;
    public Movement player;

    private void Start()
    {
        if (player != null)
        {
            player.xSensitivity = PlayerPrefs.GetFloat("xSens");
            player.ySensitivity = PlayerPrefs.GetFloat("ySens");
        }
    }

    private void OnEnable()
    {
        xSens.value = PlayerPrefs.GetFloat("xSens");
        ySens.value = PlayerPrefs.GetFloat("ySens");
    }

    private void OnDisable()
    {
        ChangeSensibility();
    }

    public void ChangeSensibility() 
    {
        if (PlayerPrefs.HasKey("xSens") || PlayerPrefs.HasKey("ySens"))
        {
            PlayerPrefs.SetFloat("xSens", xSens.value);
            PlayerPrefs.SetFloat("ySens", ySens.value);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetFloat("xSens", 3f);
            PlayerPrefs.SetFloat("ySens", 3f);
            PlayerPrefs.Save();            
        }

        if(player != null) {
            player.xSensitivity = PlayerPrefs.GetFloat("xSens");
            player.ySensitivity = PlayerPrefs.GetFloat("ySens");
        }
        
    }
}
