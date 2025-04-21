using System;
using System.Collections;
using System.Collections.Generic;
// using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
    [SerializeField] private InputButton _jumpButton;
    [SerializeField] private InputButton _attackButton;
    [SerializeField] private VariableJoystick _joystickController;
    [SerializeField] private TouchpadController _touchpadController;
    [SerializeField] private Canvas _touchCanvas;
    
    public VariableJoystick JoystickController => _joystickController;

    public TouchpadController TouchpadController => _touchpadController;

    public InputButton AttackButton => _attackButton;

    public InputButton JumpButton => _jumpButton;

    private void OnEnable()
    {
        _touchCanvas.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _touchCanvas.gameObject.SetActive(false);
    }
}
