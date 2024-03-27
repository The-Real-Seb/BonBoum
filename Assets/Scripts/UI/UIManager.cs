using System.Collections;
using System.Collections.Generic;
using BaseTemplate.Behaviours;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI _AmmoUI;
    [SerializeField] private Slider _LifeBar;

    public void ChangeAmmoText(string ammo)
    {
        _AmmoUI.text = ammo;
    }

    public void ChangeLifeValue(float life) 
    {
        _LifeBar.value = 100 - life;
    }

    public void InitLifeBar()
    {

    }
}
