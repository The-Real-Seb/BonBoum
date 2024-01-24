using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenShaker : MonoBehaviour
{
    private float _shakeTimeLeft;
    private float _shakePower;
    private Camera _cam;

    private Vector3 _originalPosition;
    
    #region Singleton

    private static ScreenShaker instance = null;
    public static ScreenShaker Instance => instance;

    #endregion
    
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
    
    private void Start()
    {
        _cam = Camera.main;
        _originalPosition = _cam.transform.localPosition;
    }

    public void Update()
    {
        if (_shakeTimeLeft > 0)
        {
            _shakeTimeLeft -= Time.deltaTime;
            Shake();
        }
        else
        {
            _cam.transform.localPosition = _originalPosition;
        }
    }

    public void SetShake(float duration, float power)
    {
        _shakeTimeLeft = duration;
        _shakePower = power;
    }

    private void Shake()
    {
        if (_shakeTimeLeft > 0)
        {
            Vector3 shakePosition = _originalPosition + Random.insideUnitSphere * _shakePower;
            _cam.transform.localPosition = shakePosition;
        }
    }
}
