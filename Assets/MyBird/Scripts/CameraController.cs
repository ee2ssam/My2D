using UnityEngine;

namespace MyBird
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

        [Header("Follow Settings")]
        [SerializeField] private bool followX = true;
        [SerializeField] private bool followY = true;
        [SerializeField] private float smoothSpeed = 5f;

        private void Start()
        {
            if (target == null)
            {
                // Try to find Player component in the scene
                var player = FindObjectOfType<Player>();
                if (player != null)
                {
                    target = player.transform;
                }
            }
        }

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = transform.position;

            if (followX) desiredPosition.x = target.position.x + offset.x;
            if (followY) desiredPosition.y = target.position.y + offset.y;

            desiredPosition.z = offset.z; // keep camera Z from offset

            transform.position = desiredPosition;            
        }
    }
}