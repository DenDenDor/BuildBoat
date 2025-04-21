using UnityEngine;

namespace CMF
{
    public class StairsWalkerController : Controller
    {
        private Mover _mover;
        private CharacterInput _characterInput;
        private AdvancedWalkerController _advancedWalkerController;
        
        private Vector3 _lastVelocity = Vector3.zero;
        public bool IsPlayerOnStair { get; set; }

        private void Start()
        {
            _mover = GetComponentInParent<Mover>();
            _characterInput = GetComponentInParent<CharacterInput>();
            _advancedWalkerController = GetComponentInParent<AdvancedWalkerController>();
        }

        private void FixedUpdate()
        {
            _advancedWalkerController.enabled = !IsPlayerOnStair;
            if (!IsPlayerOnStair) return;
            
            _lastVelocity = CalculateMovementDirection() * _advancedWalkerController.movementSpeed;
            _mover.SetVelocity(_lastVelocity);
        }

        private Vector3 CalculateMovementDirection()
        {
            if (_characterInput == null) return Vector3.zero;

            var direction = transform.up * _characterInput.GetVerticalMovementInput();
            if (direction.magnitude > 1f) direction.Normalize();

            return direction;
        }

        public override Vector3 GetVelocity() => _lastVelocity;
        public override Vector3 GetMovementVelocity() => _lastVelocity;
        public override bool IsGrounded() => false;
    }
}