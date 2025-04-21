using UnityEngine;

namespace CMF
{
    public class HoldMouseCameraController : CameraController
    {
        protected override void HandleCameraRotation()
        {
            if (Input.GetMouseButton(0))
                base.HandleCameraRotation();
        }
    }
}