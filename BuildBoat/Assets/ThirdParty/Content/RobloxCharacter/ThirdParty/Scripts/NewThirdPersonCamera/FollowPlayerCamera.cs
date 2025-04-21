using System;
using System.Collections;
using System.Collections.Generic;
using CMF;
using IE.RSB.Movement;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour, ICameraMovement
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _xAxis;
    [SerializeField] private Transform _yAxis;
    [SerializeField] private Transform _rig;

    [Header("Settings")]
    [SerializeField] private float _liftUp = 1f;
    [SerializeField] private float _minDistance = 3f;
    [SerializeField] private float _maxDistance = 10f;
    [SerializeField] private float _minAngle = -45f;
    [SerializeField] private float _maxMaxAngle = 89f;
    [SerializeField] private float _mouseWheelLerpSpeed = 5f;
    [SerializeField] private float _mouseWheelSensetivity = 1f;

    [SerializeField] private Vector2 _basicSensetivity = Vector2.one;

    [Header("Clip")]
    [SerializeField] private float _castRadius;
    [SerializeField] private LayerMask _castMask; // Убираем инициализацию здесь
    [SerializeField] private Material _controlledMaterial;
    [SerializeField] private float _minFadeDistance = 1f;
    [SerializeField] private float _maxFadeDistance = 2f;
    [SerializeField] private Transform _camera;
    
    public Transform Camera => _camera;

    private Transform Target
    {
        get
        {
            if (_target == null)
            {
                _target = FindObjectOfType<AdvancedWalkerController>().transform;
            }
            return _target;
        }
    }
    private float _sensetivityFactor = 1f;
    private float _distanceFactor = 1f;
    private float _distanceTarget = 1f;

    private Transform _transform;

    private Vector2 _angles = Vector2.zero;
    private float _distance;
    private ICameraVelocity _input;

    public bool IsInGame = true;

    public Transform Rig => _rig;

    private void Awake()
    {
        _distance = _maxDistance;
        GetTransform();

        // Инициализируем маску слоев здесь
        int invisibleWallLayer = LayerMask.NameToLayer("InvisibleWall");
        int anotherLayer = LayerMask.NameToLayer("Ignore Raycast");
        
        _castMask = ~((1 << invisibleWallLayer) | (1 << anotherLayer));    }

    private void GetTransform()
    {
        if (_transform == null)
        {
            _transform = transform;
        }
    }

    public void LookAtPoint(Vector3 point)
    {
        GetTransform();

        Vector3 direction = point - _transform.position;

        Vector2 newAngles = new Vector2();
        newAngles.y = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        newAngles.x = Mathf.Atan2(direction.y, Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z)) * Mathf.Rad2Deg;

        _angles = newAngles;
        _angles.x = Mathf.Clamp(_angles.x, _minAngle, _maxMaxAngle);

        _xAxis.localRotation = Quaternion.Euler(_angles.x, 0, 0);
        _yAxis.localRotation = Quaternion.Euler(0, _angles.y, 0);
    }

    private void LateUpdate()
    {
        _transform.position = Target.position + Vector3.up * _liftUp;
        
        UpdateRotation();
        UpdateDistance();
        UpdateFade();
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        if (IsInGame)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                UnlockCursor();
            }
        }
        else
        {
            UnlockCursor();
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void UpdateFade()
    {
        var color = Mathf.InverseLerp(_minFadeDistance, _maxFadeDistance, Mathf.Clamp(_distance, _minFadeDistance, _maxFadeDistance));
        Color col = new Color32(255, 255, 255, (byte)Mathf.Clamp(Mathf.RoundToInt(255 * color), 0, 255));
        _controlledMaterial.SetColor("_Color", col);
    }

    private void UpdateDistance()
    {
        _distanceTarget -= Input.GetAxis("Mouse ScrollWheel");
        _distanceTarget = Mathf.Clamp01(_distanceTarget);
        _distanceFactor = Mathf.Lerp(_distanceFactor, _distanceTarget, _mouseWheelLerpSpeed * Time.deltaTime);

        var distance = Mathf.Lerp(_minDistance, _maxDistance, _distanceFactor);
        _distance = Mathf.Min(_distance, distance);
        _rig.localPosition = -Vector3.forward * _distance;
    }

    private void FixedUpdate()
    {
        var distance = _maxDistance;

        // Используем обновленную маску слоев, чтобы исключить InvisibleWall
        if (Physics.SphereCast(_transform.position, _castRadius, _rig.position - _transform.position, out RaycastHit hitInfo, _maxDistance, _castMask, QueryTriggerInteraction.Ignore))
        {
            distance = hitInfo.distance - _castRadius;
        }

        _distance = Mathf.Lerp(_distance, distance, Time.fixedDeltaTime * 3f);
        _distance = Mathf.Min(_distance, distance);
    }

    public void SetSensetivity(float newValue)
    {
        newValue = Mathf.Clamp(newValue, 0.01f, 100f);
        _sensetivityFactor = newValue;
    }

    private void UpdateRotation()
    {
        Vector2 cameraInput = _input.GenerateCameraVelocity();

        var input = Vector2.left * (cameraInput.y * _basicSensetivity.y) + Vector2.up * (cameraInput.x * _basicSensetivity.x);

        _angles += input * _sensetivityFactor;

        _angles.x = Mathf.Clamp(_angles.x, _minAngle, _maxMaxAngle);

        _angles.y %= 360f;

        _xAxis.localRotation = Quaternion.Euler(_angles.x, 0, 0);
        _yAxis.localRotation = Quaternion.Euler(0, _angles.y, 0);
    }

    public void UpdateTarget(Transform target)
    {
        _target = target;
    }

    public void UpdateInput(ICameraVelocity input)
    {
        _input = input;
    }

    public void LookAtPoint2()
    {
        Debug.Log("LookAtPoint2");

        Vector3 direction = Vector3.zero - _transform.position;

        Vector2 newAngles = new Vector2();
        newAngles.y = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        newAngles.x = Mathf.Atan2(direction.y, Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z)) * Mathf.Rad2Deg;

        _angles = newAngles;
        _angles.x = Mathf.Clamp(_angles.x, _minAngle, _maxMaxAngle);

        _xAxis.localRotation = Quaternion.Euler(_angles.x, 0, 0);
        _yAxis.localRotation = Quaternion.Euler(0, _angles.y, 0);
    }
}