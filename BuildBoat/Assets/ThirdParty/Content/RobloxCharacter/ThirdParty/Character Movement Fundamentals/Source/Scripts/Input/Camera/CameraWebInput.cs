using UnityEngine;

namespace CMF
{
    public class CameraWebInput : CameraInput
    {
        public enum MouseButton { Left, Right, Middle }
        
        public CameraInputRectangle inputRect;

        public string mouseHorizontalAxis = "Mouse X";
        public string mouseVerticalAxis = "Mouse Y";
        public MouseButton mouseButton = MouseButton.Right;

        public bool invertHorizontalInput;
        public bool invertVerticalInput;
        public Vector2 mouseInputMultiplier = new(0.01f, 0.01f);

        public override float GetHorizontalCameraInput() => GetCameraInput
        (
            Input.GetAxisRaw(mouseHorizontalAxis),
            mouseInputMultiplier.x,
            -inputRect.Horizontal,
            invertHorizontalInput
        );

        public override float GetVerticalCameraInput() => GetCameraInput
        (
            -Input.GetAxisRaw(mouseVerticalAxis),
            mouseInputMultiplier.y,
            inputRect.Vertical,
            invertVerticalInput
        );

        private float GetCameraInput(float mouseInput, float inputMultiplier, float rectInput, bool isInvert)
        {
           // if (!YandexGame.SDKEnabled) return 0.0f;

            var isClicked = Input.GetMouseButton((int)mouseButton);
            
            float input;
            
            // if (YandexGame.EnvironmentData.isDesktop)
            // {
                if (isClicked)
                    input = mouseInput;
                else
                    input = 0.0f;
           // }
            // else
            // {
            //     input = rectInput;
            // }

            // if (isClicked)
            //     Cursor.lockState = CursorLockMode.Locked;
            // else
            //     Cursor.lockState = CursorLockMode.None;

            input *= inputMultiplier;
            
            if (isInvert) 
                input *= -1.0f;

            if (Time.timeScale > 0f && Time.deltaTime > 0f)
                input *= Time.timeScale / Time.deltaTime;
            else
                input *= 0f;

            return input;
        }
    }
}