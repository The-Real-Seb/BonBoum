using System;
using System.Collections;
using System.Collections.Generic;
using BaseTemplate.Behaviours;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class Movement : MonoBehaviour
{
    
    [HideInInspector]public CharacterController _controller;
    private Camera _cam;
    private float _xRotation, _yRotation;
    private Vector3 _direction, _dir;
    private float _speedMultiply = 1f;
    private Quaternion _rotation;
    public AnimationCurve fovCurve;
    public Rigidbody rb;
    
    [Header("Parameters")]
    public float speed;
    public float jumpHeight;
    public float xSensitivity = 10f;
    public float ySensitivity = 10f;
    public float gravity = -9.81f;

    [Header("Dash Parameters")] 
    public float cooldownDash = 2f;
    public float dashDuration = 0.2f;  // Durée du dash
    public float dashSpeed = 10f;      // Vitesse du dash

    private float _dashCDTime;
    private float _dashTimeLeft; 

    [Header("Feel parameters")] 
    public float camRotaPower = 1f;
    [HideInInspector]public float yVelocity;

    [Header("Network parameters")] 
    public int networkID;
    

    private void Start()
    {
        //if (!IsOwner) return;
        _controller = GetComponent<CharacterController>();
        _cam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        //if(!IsOwner) _cam.gameObject.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if(PlayerPrefs.HasKey("xSens") || PlayerPrefs.HasKey("ySens"))
        {
            xSensitivity = PlayerPrefs.GetFloat("xSens");
            ySensitivity = PlayerPrefs.GetFloat("ySens");
        }
        else
        {
            PlayerPrefs.SetFloat("xSens", 3f);
            PlayerPrefs.SetFloat("ySens", 3f);
            PlayerPrefs.Save();

            xSensitivity = PlayerPrefs.GetFloat("xSens");
            ySensitivity = PlayerPrefs.GetFloat("ySens");
        }


    }

    private bool afterDash;
    private void Update()
    {
        if(GameManager.Instance.isGamePaused) { return; }

        float fov ;
        if (_dashTimeLeft > 0)
        {
            _dashTimeLeft -= Time.deltaTime;
            _direction.y = 0;
            Vector3 dashDirection = _direction * dashSpeed;
            
            fov = Mathf.Lerp(_cam.fieldOfView, 75f, fovCurve.Evaluate(Time.deltaTime));
            

            _controller.Move(dashDirection * Time.deltaTime);  // Mouvement de dash
        }
        else
        {
            _direction = transform.forward * _dir.y + _cam.transform.right * _dir.x;
            _direction *= _speedMultiply;
            _direction.y = yVelocity;
            
            fov = Mathf.Lerp(_cam.fieldOfView, 60f, fovCurve.Evaluate(Time.deltaTime));

            if (afterDash)
            {
                PostProcessManager.Instance.AdjustChromaticAberration(0, dashDuration);
                afterDash = false;
            }

            _controller.Move(_direction * (speed * Time.deltaTime));

            if (_dashCDTime > 0)
            {
                _dashCDTime -= Time.deltaTime;
            }
            ApplyGravity();
        }
        
        _cam.fieldOfView = fov;
        //ApplyGravity();
        RunEffect();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        /*if (context.started && _dashCDTime <= 0)
        {
            _dashCDTime = cooldownDash;
            _controller.Move(transform.forward * 10);
        }*/
        
        if (context.started && _dashCDTime <= 0)
        {
            afterDash = true;
            _dashCDTime = cooldownDash;
            PostProcessManager.Instance.AdjustChromaticAberration(0.3f, dashDuration);
            _dashTimeLeft = dashDuration;  // Commencer le dash
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        _dir = context.ReadValue<Vector2>();
    }

    public void Aim(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.isGamePaused) { return; }

        Vector2 mouseMove = context.ReadValue<Vector2>();

        _xRotation += mouseMove.x * Time.deltaTime * xSensitivity;
        _yRotation -= mouseMove.y * Time.deltaTime * ySensitivity;
        _yRotation = Mathf.Clamp(_yRotation, -90, 90);

        transform.rotation = Quaternion.Euler(0, _xRotation, 0);
        _cam.transform.localRotation = Quaternion.Euler(_yRotation, 0, 0);
        
    }
    public void Run(InputAction.CallbackContext context)
    {
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
        if (GameManager.Instance.isGamePaused) { return; }
        //if (!IsOwner) return;
        if (IsGrounded() && context.performed)
        {
            yVelocity = jumpHeight;
        }
    }
    private void ApplyGravity()
    {
        if (GameManager.Instance.isGamePaused) { return; }
        //if (!IsOwner) return;
        if (IsGrounded()) yVelocity = -0.5f;
        else yVelocity += gravity * Time.deltaTime;
    }
    
    public bool IsGrounded() => _controller.isGrounded;
    
}
