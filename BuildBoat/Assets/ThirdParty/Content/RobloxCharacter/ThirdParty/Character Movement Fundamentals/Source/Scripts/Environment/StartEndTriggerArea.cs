using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    public class StartEndTriggerArea : MonoBehaviour
    {
        public StartEndMovingPlatform movingPlatform;
        public List<Rigidbody> rigidbodiesInTriggerArea = new();
        
        private void OnTriggerEnter(Collider col)
        {
            if (col.attachedRigidbody == null || col.GetComponent<Mover>() == null) return;
            
            rigidbodiesInTriggerArea.Add(col.attachedRigidbody);
            movingPlatform.CurrentWaypoint = movingPlatform.endWaypoint;
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.attachedRigidbody == null || col.GetComponent<Mover>() == null) return;
            
            rigidbodiesInTriggerArea.Remove(col.attachedRigidbody);
            movingPlatform.CurrentWaypoint = movingPlatform.startWaypoint;
        }
    }
}