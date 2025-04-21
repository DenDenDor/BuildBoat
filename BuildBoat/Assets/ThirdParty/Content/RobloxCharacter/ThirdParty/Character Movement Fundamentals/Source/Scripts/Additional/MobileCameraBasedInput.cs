using CMF;
using Goossyaa;
using Goossyaa.Third_party.Content.Character_Movement_Fundamentals.Source.Scripts.Input;
using UnityEngine;

namespace IE.RSB.Movement
{
    public class MobileCameraBasedInput : ICameraVelocity
    {
        public Vector2 GenerateCameraVelocity()
        {
           // CameraInput cameraInput = Object.FindObjectOfType<CameraController>().CameraInput;

           Vector2 direction = new Vector2(TouchControls_ManipulateCharacter.Instance.GetHorizontalCameraInput(), 
               -TouchControls_ManipulateCharacter.Instance.GetVerticalCameraInput());
            
            return direction * 10;
        }
    }
}