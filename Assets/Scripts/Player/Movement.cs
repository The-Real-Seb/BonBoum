using System;
using System.Collections;
using System.Collections.Generic;
using BaseTemplate.Behaviours;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : NetworkBehaviour
{
    
    [HideInInspector]public CharacterController _controller;
    private Camera _cam;
    private float _xRotation, _yRotation;
    private Vector3 _direction, _dir;
    private float _speedMultiply = 1f;
    private Quaternion _rotation;
    public AnimationCurve fovCurve;
    
    [Header("Parameters")]
    public float speed;
    public float jumpHeight;
    public float xSensitivity = 10f;
    public float ySensitivity = 10f;
    public float gravity = -9.81f;

    [Header("Feel parameters")] 
    public float camRotaPower = 1f;
    [HideInInspector]public float yVelocity;

    [Header("Network parameters")] 
    public int networkID;
    

    private void Start()
    {
        Debug.Log($"Start - IsOwner: {IsOwner}");
        //if (!IsOwner) return;
        _controller = GetComponent<CharacterController>();
        _cam = GetComponentInChildren<Camera>();
        
        if(!IsOwner) _cam.gameObject.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        SendPositionServerRpc(transform.position, transform.rotation);
        
        _direction = transform.forward * _dir.y + _cam.transform.right * _dir.x;
        _direction *= _speedMultiply;
        _direction.y = yVelocity;
        _controller.Move(_direction * (speed * Time.deltaTime));
        
        ApplyGravity();
        RunEffect();
    }

    public void Move(InputAction.CallbackContext context)
    {
        _dir = context.ReadValue<Vector2>();
    }

    public void Aim(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        
        Vector2 mouseMove = context.ReadValue<Vector2>();

        _xRotation += mouseMove.x * Time.deltaTime * xSensitivity;
        _yRotation -= mouseMove.y * Time.deltaTime * ySensitivity;
        _yRotation = Mathf.Clamp(_yRotation, -90, 90);

        transform.rotation = Quaternion.Euler(0, _xRotation, 0);
        _cam.transform.localRotation = Quaternion.Euler(_yRotation, 0, 0);
        
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.started)
        {
            _speedMultiply = 2.5f;
        }

        if (context.canceled)
        {
            _speedMultiply = 1f;
        }
    }

    private void RunEffect()
    {
        if (!IsOwner) return;
        if (_speedMultiply > 1f)
        {
            if (_dir.magnitude > 0)
            {
                float fov = Mathf.Lerp(_cam.fieldOfView, 70f, fovCurve.Evaluate(Time.deltaTime));
                _cam.fieldOfView = fov;
            }
            else
            {
                float fov = Mathf.Lerp(_cam.fieldOfView, 60f, fovCurve.Evaluate(Time.deltaTime));
                _cam.fieldOfView = fov;
            }
        }
        else
        {
            float fov = Mathf.Lerp(_cam.fieldOfView, 60f, fovCurve.Evaluate(Time.deltaTime));
            _cam.fieldOfView = fov;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (IsGrounded() && context.performed)
        {
            yVelocity = jumpHeight;
        }
    }
    
    private void ApplyGravity()
    {
        if (!IsOwner) return;
        if (IsGrounded()) yVelocity = -0.5f;
        else yVelocity += gravity * Time.deltaTime;
    }
    
    public bool IsGrounded() => _controller.isGrounded;
    
    [ServerRpc]
    public void SendPositionServerRpc(Vector3 position, Quaternion rotation)
    {
        UpdatePositionClientRpc(position, rotation);
    }

    [ClientRpc]
    public void UpdatePositionClientRpc(Vector3 position, Quaternion rotation)
    {
        if (IsOwner) return;
        transform.position = position;
        transform.rotation = rotation;
    }

}
