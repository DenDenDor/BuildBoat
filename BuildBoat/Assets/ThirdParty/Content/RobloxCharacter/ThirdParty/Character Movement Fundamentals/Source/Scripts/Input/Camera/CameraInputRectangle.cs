using UnityEngine;
using UnityEngine.EventSystems;

namespace CMF
{
    public class CameraInputRectangle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField, Min(0.01f)] private Vector2 mobileCameraInputMultiplier;
        
        private Vector2 _resultDelta;
        private Vector2 _lastTouchPosition;
        private bool _isPressed;
        private int _fingerId;

        public float Horizontal => _resultDelta.x;
        public float Vertical => _resultDelta.y;

        private void Update()
        {
            if (!_isPressed) return;
            foreach (var touch in Input.touches)
            {
                if (touch.fingerId != _fingerId) continue;
                
                if (touch.deltaPosition.magnitude > 125.0f) continue;
                
                _resultDelta = touch.phase == TouchPhase.Moved
                    ? Vector2.Scale(touch.deltaPosition, mobileCameraInputMultiplier) 
                    : Vector2.zero;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            _fingerId = eventData.pointerId;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _resultDelta = Vector2.zero;
            _isPressed = false;
        }
    }
}