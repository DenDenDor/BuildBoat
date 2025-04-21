using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    public class BaseMovingPlatform : MonoBehaviour
    {
        public float movementSpeed = 10f;
        public bool reverseDirection;
        public float waitTime = 1f;
        public List<Transform> waypoints = new();

        protected TriggerArea TriggerArea;
        protected Rigidbody Rb;

        private bool _isWaiting;
        private int _currentWaypointIndex;
        private Transform _currentWaypoint;

        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            TriggerArea = GetComponentInChildren<TriggerArea>();

            Rb.freezeRotation = true;
            Rb.useGravity = false;
            Rb.isKinematic = true;
        }

        public void StartMovingPlatform()
        {
            _isWaiting = true;
            
            if (waypoints.Count <= 0)
                Debug.LogWarning("No waypoints have been assigned to 'MovingPlatform'!");
            else
                _currentWaypoint = waypoints[_currentWaypointIndex];

            StartCoroutine(WaitRoutine());
            StartCoroutine(LateFixedUpdate());
        }

        private IEnumerator LateFixedUpdate()
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
            if (waypoints.Count <= 0 || _isWaiting) return;

            var toCurrentWaypoint = _currentWaypoint.position - transform.position;
            var movement = toCurrentWaypoint.normalized * (movementSpeed * Time.fixedDeltaTime);

            if (movement.magnitude >= toCurrentWaypoint.magnitude || movement.magnitude == 0f)
            {
                Rb.transform.position = _currentWaypoint.position;
                UpdateWaypoint();
            }
            else
            {
                Rb.transform.position += movement;
            }

            if (TriggerArea == null) return;

            foreach (var rb in TriggerArea.rigidbodiesInTriggerArea) 
                rb.MovePosition(rb.position + movement);
        }

        private void UpdateWaypoint()
        {
            _currentWaypointIndex += reverseDirection ? -1 : 1;

            if (_currentWaypointIndex >= waypoints.Count) _currentWaypointIndex = 0;
            if (_currentWaypointIndex < 0) _currentWaypointIndex = waypoints.Count - 1;

            _currentWaypoint = waypoints[_currentWaypointIndex];
            _isWaiting = true;
            
            if (_currentWaypointIndex == 0) StopAllCoroutines();
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