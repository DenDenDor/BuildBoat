using System.Collections;
using CMF;
using UnityEngine;

namespace Goossyaa.Third_party.Content.Character_Movement_Fundamentals.Source.Scripts.Input.Character
{
    public class BotInput : CharacterInput
    {
        private Vector2 _current;

        private Vector2 _target;

        private bool _isJumping;

        private Coroutine _correcting;

        public void MoveByJoystickInput(Vector2 input)
        {
            // _target = input;
            //
            // _correcting ??= StartCoroutine(CorrectingToTarget());

            if (_correcting != null)
            {
                StopCoroutine(_correcting);

                _correcting = null;
            }

            _current = input;
        }

        public void SetJumping(bool isJumping)
        {
            _isJumping = isJumping;
        }

        public void StopByJoystickInput()
        {
            if (_correcting == null)
                _correcting = StartCoroutine(CorrectingToStop());
            
            _current = Vector2.zero;
        }

        public override float GetHorizontalMovementInput()
        {
            return _current.x;
        }

        public override float GetVerticalMovementInput()
        {
            return _current.y;
        }

        public override bool IsJumpKeyPressed()
        {
            return _isJumping;
        }

        private IEnumerator CorrectingToStop()
        {
            while (Vector2.Distance(_target, Vector2.zero) > 0)
            {
                _current = Vector2.MoveTowards(_current, Vector2.zero, .1f * Time.deltaTime);

                yield return null;
            }

            _correcting = null;
        }
    }
}