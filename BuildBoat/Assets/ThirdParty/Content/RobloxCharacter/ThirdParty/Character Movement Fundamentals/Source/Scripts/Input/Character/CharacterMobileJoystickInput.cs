using Goossyaa.Third_party.Content.Character_Movement_Fundamentals.Source.Scripts.Input;
using UnityEngine;

namespace CMF
{
    public class CharacterMobileJoystickInput : CharacterInput
    {
     //   [SerializeField] private CharacterMobileJoystickRectangle joystickRect;
     //   [SerializeField] private CharacterMobileJumpButtonRectangle jumpButton; 
        
        public string horizontalInputAxis = "Horizontal";
        public string verticalInputAxis = "Vertical";
        public KeyCode jumpKey = KeyCode.Space;
        public bool useRawInput = true;

        public Vector2 ConvertDirection() => new Vector2(GetHorizontalMovementInput(), GetVerticalMovementInput());
        
        public override float GetHorizontalMovementInput()
        {
            return RewiredAxes.MoveX;
            
            return 0;
            
          //  if (!YandexGame.SDKEnabled) return 0.0f;
         //   if (!YandexGame.EnvironmentData.isDesktop) return joystickRect.Horizontal;
            
            return useRawInput ? Input.GetAxisRaw(horizontalInputAxis) : Input.GetAxis(horizontalInputAxis);
        }

        public override float GetVerticalMovementInput()
        {
            return RewiredAxes.MoveY;
            
          //  if (!YandexGame.SDKEnabled) return 0.0f;
          //  if (!YandexGame.EnvironmentData.isDesktop) return joystickRect.Vertical;

            return useRawInput ? Input.GetAxisRaw(verticalInputAxis) : Input.GetAxis(verticalInputAxis);
        }

        public override bool IsJumpKeyPressed()
        {
            return Input.GetKey(jumpKey);
        }
    }
}