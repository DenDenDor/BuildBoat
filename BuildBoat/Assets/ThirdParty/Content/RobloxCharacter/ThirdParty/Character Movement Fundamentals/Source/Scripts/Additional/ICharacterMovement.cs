using IE.RSB.Movement;

namespace Source.Common.Code.Scripts.Level.Player.Movement
{
    public interface ICharacterMovement
    {
        void UpdateVelocity(ICharacterVelocity velocity);
    }
}