using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectWeapon : MonoBehaviour
{
    public List<Pistol> pistols;   

    public int selectedWeaponIndex = 0;


    public void TriggerFire(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.isGamePaused) { return; }
        pistols[selectedWeaponIndex].Fire(context);
    }

    public void InputReload(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.isGamePaused) { return; }
        if (context.started)
        {
            StartCoroutine(pistols[selectedWeaponIndex].Reload());
        }
    }

    public void SwitchWeapon(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.isGamePaused) { return; }
        if (context.performed)
        {
            float scrollValue = context.ReadValue<float>();

            pistols[selectedWeaponIndex].gameObject.SetActive(false);
            if (scrollValue > 0) 
            { 
                selectedWeaponIndex++;
                selectedWeaponIndex = Mathf.Clamp(selectedWeaponIndex, 0, pistols.Count-1);
            }
            else if (scrollValue < 0)
            {
                selectedWeaponIndex--;
                selectedWeaponIndex = Mathf.Clamp(selectedWeaponIndex, 0, pistols.Count-1);
            }
            pistols[selectedWeaponIndex].gameObject.SetActive(true);
            Debug.Log(selectedWeaponIndex);
        }
        
    }
}
