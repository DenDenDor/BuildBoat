using System;
using Goossyaa.Third_party.Content.Character_Movement_Fundamentals.Source.Scripts.Input;
using UnityEngine;

namespace CMF
{
    public class MobileCameraInput : CameraInput
    {
        public override float GetHorizontalCameraInput()
        {
            return RewiredAxes.RotationX;
        }

        public override float GetVerticalCameraInput()
        {
            return RewiredAxes.RotationY;
        }
    }
}