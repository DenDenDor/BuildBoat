using UnityEngine;

namespace CMF
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEnterPlatform : MonoBehaviour
    {
        [SerializeField] private BaseMovingPlatform platform;

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody != null && other.GetComponent<Mover>() != null)
            {
                platform.StartMovingPlatform();
            }
        }
    }
}