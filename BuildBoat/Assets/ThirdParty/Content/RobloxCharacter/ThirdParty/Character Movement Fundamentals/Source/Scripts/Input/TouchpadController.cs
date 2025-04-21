using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchpadController : MonoBehaviour
{
 public float rotationSpeed = 0.5f;
    public RectTransform rotationArea; // Область, на которой разрешен поворот камеры

    private Vector2 touchStartPosition;
    private bool isRotating = false;

    private Vector2 _direction;
    
    public Vector2 Direction => _direction;
    
    void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (IsTouchWithinRightHalfScreen(touch.position))
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            if (IsTouchWithinRotationArea(touch.position))
                            {
                                touchStartPosition = touch.position;
                                isRotating = true;
                            }
                            break;

                        case TouchPhase.Moved:
                            if (isRotating)
                            {
                                Vector2 touchDeltaPosition = touch.position - touchStartPosition;
                                _direction = (touchDeltaPosition);
                                touchStartPosition = touch.position;
                            }
                            break;

                        case TouchPhase.Ended:
                            _direction = Vector2.zero;
                            isRotating = false;
                            break;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (IsTouchWithinRightHalfScreen(Input.mousePosition) && IsTouchWithinRotationArea(Input.mousePosition))
            {
                touchStartPosition = Input.mousePosition;
                isRotating = true;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (isRotating)
            {
                Vector2 mouseDeltaPosition = (Vector2)Input.mousePosition - touchStartPosition;
                _direction = (mouseDeltaPosition);
                touchStartPosition = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
            _direction = Vector2.zero;
        }
    }

    private bool IsTouchWithinRotationArea(Vector2 touchPosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rotationArea, touchPosition, null, out localPoint);
        return rotationArea.rect.Contains(localPoint);
    }

    private bool IsTouchWithinRightHalfScreen(Vector2 touchPosition)
    {
        return touchPosition.x > Screen.width / 2;
    }    
    
}
