using UnityEngine;

namespace Source.Common.Code.Scripts.Level.Player.Movement
{
    public interface ICharacterVelocity
    {
        Vector2 GenerateCharacterVelocity();
    }
}