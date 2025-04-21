using UnityEngine;
using System.Collections;

namespace CMF
{
    public class RotatingPlatform : MonoBehaviour
    {
        public float rotationSpeed = 15f;
        public bool reverseDirection;
        public Vector3 axis = new Vector3(0,1,0);


        private Rigidbody _r;
        private RotationPlatformTriggerArea _triggerArea;
        private float _currentAngle;

        private float CurrentAngle
        {
            get => _currentAngle;
            set
            {
                _currentAngle = value;
                
                if (_currentAngle > 360.0f) _currentAngle -= 360.0f;
                if (_currentAngle < 0.0f) _currentAngle += 360.0f;
            }
        }

        private void Start()
        {
            _r = GetComponent<Rigidbody>();
            _triggerArea = GetComponentInChildren<RotationPlatformTriggerArea>();

            _r.freezeRotation = false;
            _r.useGravity = false;
            _r.isKinematic = true;

            StartCoroutine(LateFixedUpdate());
        }

        private IEnumerator LateFixedUpdate()
        {
            var instruction = new WaitForFixedUpdate();
            while (true)
            {
                yield return instruction;
                RotatePlatform();
            }
        }

        private void RotatePlatform()
        {
            var rotationAmount = rotationSpeed * Time.fixedDeltaTime * (reverseDirection ? -1.0f : 1.0f);
            CurrentAngle += rotationAmount;

            _r.rotation = Quaternion.AngleAxis(CurrentAngle, axis);

            if (_triggerArea.rigidBodiesOnPlatform.Count != _triggerArea.rigidBodiesOnPlatformWithTime.Count)
            {
                _triggerArea.rigidBodiesOnPlatformWithTime.Clear();
                foreach (var rb in _triggerArea.rigidBodiesOnPlatform)
                    _triggerArea.rigidBodiesOnPlatformWithTime.Add(rb, 1.0f);
            }

            foreach (var rb in _triggerArea.rigidBodiesOnPlatform)
            {
                if (!_triggerArea.rigidBodiesOnPlatformWithTime.TryGetValue(rb, out var timer)) continue;
                switch (timer)
                {
                    case < 1.0f: _triggerArea.rigidBodiesOnPlatformWithTime[rb] += Time.fixedDeltaTime * 4.0f; break;
                    case > 1.0f: _triggerArea.rigidBodiesOnPlatformWithTime[rb] = 1.0f; break;
                }
                RotateRigidBodiesOnPlatform(rb, rotationAmount * timer);
            }
        }

        private void RotateRigidBodiesOnPlatform(Rigidbody rb, float rotationAmount)
        {
            var localAngleAxis = Quaternion.AngleAxis(rotationAmount, transform.up);
            rb.position = localAngleAxis * (rb.position - _r.position) + _r.position;

            var globalAngleAxis = Quaternion.AngleAxis(rotationAmount, rb.transform.InverseTransformDirection(transform.up));
            rb.rotation *= globalAngleAxis;
        }
    }
}