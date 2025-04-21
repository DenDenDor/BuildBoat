using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    [RequireComponent(typeof(Collider))]
    public class RotationPlatformTriggerArea : MonoBehaviour
    {
        public List<Rigidbody> rigidBodiesOnPlatform = new();
        public Dictionary<Rigidbody, float> rigidBodiesOnPlatformWithTime = new();
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody == null || other.GetComponent<Mover>() == null) return;
            
            rigidBodiesOnPlatform.Add(other.attachedRigidbody);
            rigidBodiesOnPlatformWithTime.Add(other.attachedRigidbody, 0.0f);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.attachedRigidbody == null || other.GetComponent<Mover>() == null) return;
            
            rigidBodiesOnPlatform.Remove(other.attachedRigidbody);
            rigidBodiesOnPlatformWithTime.Remove(other.attachedRigidbody);
        }
    }
}