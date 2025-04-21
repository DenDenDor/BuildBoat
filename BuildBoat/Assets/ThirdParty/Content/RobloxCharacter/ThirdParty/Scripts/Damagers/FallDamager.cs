using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class FallDamager : MonoBehaviour
{
    [SerializeField] private float _verticalSpeedDamageFactor = 1.0f;
    [SerializeField] private float _horizontalSpeedDamageFactor = 0.2f;

    [SerializeField] private float _minimalDamage = 10;
    [SerializeField] private float _maximalDamage = 100f;
    [SerializeField] private float _minimalDamageSpeed = 15;
    [SerializeField] private float _maximalDamageSpeed = 1000;

    private Rigidbody _rigidbody;
    private Health _health;
    private CameraShake _cameraShake;
    private Vector3 _speed;

    private bool _isInCooldown;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _health = GetComponent<Health>();
        _cameraShake = FindFirstObjectByType<CameraShake>();
    }

    private void Update()
    {
        var currentSpeed = _rigidbody.linearVelocity;

        currentSpeed.y = Mathf.Min(currentSpeed.y, 0) * _verticalSpeedDamageFactor; // only down
        currentSpeed.x *= _horizontalSpeedDamageFactor;
        currentSpeed.z *= _horizontalSpeedDamageFactor;

        var resultSpeedDelta = (_speed - currentSpeed).magnitude; // can be changed to sqr magnitude
        var damage = resultSpeedDelta.RemapRaza(_minimalDamageSpeed, _maximalDamageSpeed, _minimalDamage, _maximalDamage);

        if (resultSpeedDelta >= _minimalDamageSpeed)
        {
            _health.TakeDamage(damage);

            if (_cameraShake != null)
                _cameraShake.Shake(
                    amplitude: resultSpeedDelta.RemapRaza(_minimalDamageSpeed, _maximalDamageSpeed, 0f, 1f) + 1);
        }
        
        _speed = currentSpeed;
    }
}
