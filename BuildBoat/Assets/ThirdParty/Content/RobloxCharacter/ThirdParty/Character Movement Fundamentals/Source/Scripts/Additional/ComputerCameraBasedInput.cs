using Goossyaa;
using UnityEngine;

namespace IE.RSB.Movement
{
    public class ComputerCameraBasedInput : ICameraVelocity
    {
        public Vector2 GenerateCameraVelocity()
        {
            Vector2 input = Vector2.zero;

            if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                input = new Vector2(mouseX, mouseY) * 2.2f;
            }

            return input;
        }
    }
}