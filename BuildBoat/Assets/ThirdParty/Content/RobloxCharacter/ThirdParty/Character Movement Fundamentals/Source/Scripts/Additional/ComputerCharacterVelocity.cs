using Goossyaa;
using UnityEngine;

namespace Source.Common.Code.Scripts.Level.Player.Movement
{
    public class ComputerCharacterVelocity : ICharacterVelocity
    {
        public Vector2 GenerateCharacterVelocity()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }
}