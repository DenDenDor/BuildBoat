using System;
using IE.RSB.Movement;
using Source.Common.Code.Scripts.Level.Player.Movement;

namespace Sourse.Common.Code.Rewired.Router
{
    public class RewiredRouter 
    {
       private readonly ICameraMovement _cameraMovement;
        private readonly ICharacterMovement _characterMovement;
        
        public RewiredRouter(ICameraMovement cameraMovement, ICharacterMovement characterMovement, Func<(ICameraVelocity, ICharacterVelocity)> generateCameraVelocity, Action switchActivityRewired)
        {
            _cameraMovement = cameraMovement;
            _characterMovement = characterMovement;

            var item = generateCameraVelocity();

            _cameraMovement.UpdateInput(item.Item1);
            
            _characterMovement.UpdateVelocity(item.Item2);
            
            switchActivityRewired();
        }

        public void Exit()
        {
            
        }
    }
}