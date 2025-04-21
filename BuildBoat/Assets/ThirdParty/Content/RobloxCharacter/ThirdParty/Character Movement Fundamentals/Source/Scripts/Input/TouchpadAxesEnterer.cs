using System;
using UnityEngine;

namespace Goossyaa.Third_party.Content.Character_Movement_Fundamentals.Source.Scripts.Input
{
    public class TouchpadAxesEnterer : MonoBehaviour
    {
        private const float RotateSensitivity = .6f;

        private const bool VerticalRotateInverted = true;
        
       // private readonly SaverSensitivity _saverSensitivity = new(new JsonSaverData<SensitivityData>());
        
        public event Action Triggered;

        private Vector2 _lastInput;

        [SerializeField] private TouchpadController  _touchPad;
        [SerializeField] private float _value = 2;

        public event Action ClickedOnButton;
        public TouchpadController TouchPad => _touchPad;

        private void Awake()
        {

        }

        public void UpMouse()
        {
            Debug.Log("UpMouse");
        }
        
        public void DownMouse()
        {
            Debug.Log("DownMouse");
        }

        public void Click()
        {
            Debug.Log("Click");
            ClickedOnButton?.Invoke();   
        }
        
        public void Tap(Vector2 input)
        {
//            Debug.Log("Tap " + input);
            if (_lastInput != input)
            {
                Triggered?.Invoke();
            }

            _lastInput = input;
        }

        public float GetValue()
        {
            float value = 0.5f;

            //_factorySaverSensitivity.Create().TryLoad(out value);

            return value;
        }

        public void EnterTouchpad(Vector2 input)
        {
            float value = 0.5f;

           // _factorySaverSensitivity.Create().TryLoad(out value);

            if (value <= 0.05)
            {
                value = 0.05f;
            }

            Debug.Log(value + " Sensetivity");

           // FindObjectOfType<PlayerCameraController>().UpdateSensitivity(value * _value);

            var normalized = input * value * _value;
            
            RewiredAxes.RotationX = normalized.x;
            
            RewiredAxes.RotationY = normalized.y * (VerticalRotateInverted ? -1 : 1);
        }

        public void EnterJoystick(Vector2 input)
        {
            RewiredAxes.MoveX = input.x;
            
            RewiredAxes.MoveY = input.y;
        }

    }
}