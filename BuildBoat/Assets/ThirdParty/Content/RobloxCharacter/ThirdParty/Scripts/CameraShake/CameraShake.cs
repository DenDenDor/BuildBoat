using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private AnimationCurve _shakeCurve;
    [SerializeField] private Vector3 _shakeAmplitude = Vector3.one;
    [SerializeField] private Vector3 _shakeSpeed = Vector3.one;
    private Transform _transform;
    private List<Shaker> _shakes = new List<Shaker>();

    public event Action Shaked;
    

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        Quaternion result = Quaternion.identity;

        for (int i = _shakes.Count - 1; i >= 0; i--)
        {
            if (_shakes[i] == null)
            {
                _shakes.RemoveAt(i);
                continue;
            }

            if (_shakes[i].IsCompleted)
            {
                _shakes.RemoveAt(i);
                continue;
            }

            result *= _shakes[i].Update().Result;
        }

        _transform.localRotation = result;

    }

    public void Shake(float duration = 0.5f, float amplitude = 1f)
    {
        if (duration <= 0.01f)
            duration = 0.01f;
        
        Shaked?.Invoke();

        _shakes.Add(new Shaker(duration, _shakeAmplitude * amplitude, _shakeSpeed, _shakeCurve));
    }

    [ContextMenu("MakeShake")]
    private void MakeShake()
    {
        Shake();
    }

    [System.Serializable]
    private class Shaker
    {
        private float _shakeTimer;
        private float _currentDuration;
        private Vector3 _shakeAmplitude = Vector3.one;
        private Vector3 _shakeSpeed = Vector3.one;
        private AnimationCurve _shakeCurve;
        private Quaternion _result = Quaternion.identity;

        public bool IsCompleted => _shakeTimer < 0;
        public Quaternion Result => _result;

        public Shaker(float shakeTime, Vector3 shakeAmplitude, Vector3 shakeSpeed, AnimationCurve shakeCurve)
        {
            _shakeTimer = shakeTime;
            _currentDuration = shakeTime;
            _shakeAmplitude = shakeAmplitude;
            _shakeSpeed = shakeSpeed;
            _shakeCurve = shakeCurve;
        }


        public Shaker Update()
        {
            _shakeTimer -= Time.deltaTime;

            if (_shakeTimer >= 0)
            {
                var result = _shakeCurve.Evaluate(Mathf.Clamp01(_shakeTimer / _currentDuration));
                Vector3 amplitude = Vector3.zero;
                amplitude.x = (Mathf.PerlinNoise(Time.time * _shakeSpeed.x, 0) - 0.5f) * _shakeAmplitude.x;
                amplitude.y = (Mathf.PerlinNoise(0, Time.time * _shakeSpeed.y) - 0.5f) * _shakeAmplitude.y;
                amplitude.z = (Mathf.PerlinNoise(Time.time * _shakeSpeed.z, Time.time * _shakeSpeed.z) - 0.5f) * _shakeAmplitude.z;

                Vector3 shake = amplitude * result * 2f;
                _result = Quaternion.Euler(shake);
            }
            else
            {
                _result = Quaternion.identity;
            }

            return this;
        }

    }
}
