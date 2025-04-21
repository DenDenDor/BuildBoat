using UnityEngine;
using System.Collections;

namespace CMF
{
    //This script moves a rigidbody along a set of waypoints;
    //It also moves any controllers on top along with it;
    public class StartEndMovingPlatform : MonoBehaviour
    {
        public float movementSpeed = 10.0f;
        public float waitTime = 1.5f;
        public Transform startWaypoint;
        public Transform endWaypoint;

        private Rigidbody _r;
        private StartEndTriggerArea _triggerArea;

        private bool _isWaiting;
        private Transform _currentWaypoint;

        public Transform CurrentWaypoint
        {
            get => _currentWaypoint;
            set
            {
                _currentWaypoint = value;
                _isWaiting = true;
            }
        }

        private void Start()
        {
            _r = GetComponent<Rigidbody>();
            _triggerArea = GetComponentInChildren<StartEndTriggerArea>();

            _r.freezeRotation = true;
            _r.useGravity = false;
            _r.isKinematic = true;

            CurrentWaypoint = startWaypoint;

            StartCoroutine(WaitRoutine());
            StartCoroutine(LateFixedUpdateRoutine());
        }

        private IEnumerator LateFixedUpdateRoutine()
        {
            var instruction = new WaitForFixedUpdate();
            while (true)
            {
                yield return instruction;
                MovePlatform();
            }
        }

        private void MovePlatform()
        {
            if (_isWaiting) return;

            var toCurrentWaypoint = CurrentWaypoint.position - transform.position;
            var movement = toCurrentWaypoint.normalized;
            movement *= movementSpeed * Time.deltaTime;

            if (movement.magnitude >= toCurrentWaypoint.magnitude || movement.magnitude == 0f)
                _r.transform.position = CurrentWaypoint.position;
            else
                _r.transform.position += movement;

            if (_triggerArea == null) return;

            foreach (var rb in _triggerArea.rigidbodiesInTriggerArea)
                rb.MovePosition(rb.position + movement);
        }

        private IEnumerator WaitRoutine()
        {
            var waitInstruction = new WaitForSeconds(waitTime);
            while (true)
            {
                if (_isWaiting)
                {
                    yield return waitInstruction;
                    _isWaiting = false;
                }

                yield return null;
            }
        }
    }
}