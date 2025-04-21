using UnityEngine;

namespace CMF
{
    public class StairsTrigger : MonoBehaviour
    {
        public Transform exitZone;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<StairsWalkerController>(out var stairsController)) return;
            OnPlayerEnter(stairsController);
        }

        protected virtual void OnPlayerEnter(StairsWalkerController stairsController)
        {
            var rb = stairsController.GetComponentInParent<Rigidbody>();
            if (rb == null) return;
            
            stairsController.IsPlayerOnStair = !stairsController.IsPlayerOnStair;
            rb.position = stairsController.IsPlayerOnStair ? transform.position : exitZone.position;
        }
    }
}