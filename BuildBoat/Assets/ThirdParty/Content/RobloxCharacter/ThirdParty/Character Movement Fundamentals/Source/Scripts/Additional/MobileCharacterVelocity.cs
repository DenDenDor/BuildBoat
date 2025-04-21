using Goossyaa;
using UnityEngine;

namespace Source.Common.Code.Scripts.Level.Player.Movement
{
    public class MobileCharacterVelocity : ICharacterVelocity
    {
        public Vector2 GenerateCharacterVelocity()
        {
            Vector2 velocity =new Vector2(TouchControls_ManipulateCharacter.Instance.GetHorizontalMovementInput(), 
                TouchControls_ManipulateCharacter.Instance.GetVerticalMovementInput());

            return velocity;

        }
    }
}