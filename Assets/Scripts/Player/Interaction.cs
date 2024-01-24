using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public LayerMask layerMask;
    private Camera _camera;
    private bool canInteract;
    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        SetAim();
    }

    private void SetAim()                                                                                                                                                                                             
    {
        //Tir d'un rayon
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,layerMask))
        {
            if (hit.transform.TryGetComponent<IInteractable>(out IInteractable outComp))
            {
                if(canInteract)
                    outComp.Interact();
            }
        }
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            canInteract = true;
        }
        
        if (context.canceled)
        {
            canInteract = false;
        }
    }
}
