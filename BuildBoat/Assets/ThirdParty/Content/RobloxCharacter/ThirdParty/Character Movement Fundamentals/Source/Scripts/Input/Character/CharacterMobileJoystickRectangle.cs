using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CMF
{
    public class CharacterMobileJoystickRectangle : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
    {
        [SerializeField, Min(10.0f)] private float maxClampedDistance = 50.0f;
        [SerializeField] private Image backgroundJoystickImage;
        [SerializeField] private Image handleJoystickImage;
        [SerializeField] private bool hideGraphics = true;
        [SerializeField] private bool rotateHandle = true;
        [SerializeField] private bool normalize = true;

        private Vector2 _startJoystickPosition;
        private Quaternion _startHandleRotation;
        private Vector2 _joystickPosition;
        private bool _joystickActive;
        
        private Vector2 _resultValue;

        public float Horizontal => _resultValue.x;
        public float Vertical => _resultValue.y;

        private void Start()
        {
            _startJoystickPosition = backgroundJoystickImage.rectTransform.position;
            _startHandleRotation = handleJoystickImage.rectTransform.rotation;
            SetJoystickActive(false);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_joystickActive is false) return;

            var distance = Vector2.Distance(eventData.position, _joystickPosition);
            var clampedDistance = Mathf.Clamp(distance, 0, maxClampedDistance);
            var direction = (eventData.position - _joystickPosition).normalized * clampedDistance;

            handleJoystickImage.rectTransform.position = _joystickPosition + direction;
            var clampedDirection = direction / maxClampedDistance;
            if (normalize) clampedDirection.Normalize();

            _resultValue = clampedDirection;

            if (rotateHandle) handleJoystickImage.rectTransform.right = direction;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetJoystickActive(true);

            _joystickPosition = eventData.position;
            backgroundJoystickImage.rectTransform.position = _joystickPosition;
            handleJoystickImage.rectTransform.position = _joystickPosition;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _resultValue = Vector2.zero;

            backgroundJoystickImage.rectTransform.position = _startJoystickPosition;
            handleJoystickImage.rectTransform.position = _startJoystickPosition;
            handleJoystickImage.rectTransform.rotation = _startHandleRotation;

            SetJoystickActive(false);
        }

        private void SetJoystickActive(bool value)
        {
            if (hideGraphics)
            {
                backgroundJoystickImage.enabled = value;
                handleJoystickImage.enabled = value;
            }

            _joystickActive = value;
        }
    }
}