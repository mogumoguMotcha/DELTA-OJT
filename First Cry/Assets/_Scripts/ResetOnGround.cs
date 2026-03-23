using UnityEngine;

namespace Delivery_Room.Script
{
    public class ResetOnGround : MonoBehaviour
    {
        private Vector3 startPosition;
        private Quaternion startRotation;

        void Start()
        {
            // Save the original position & rotation
            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        void OnCollisionEnter(Collision collision)
        {
            // Check if it hit the Ground (by Tag)
            if (collision.gameObject.CompareTag("Ground"))
            {
                ResetObject();
            }
        }

        void ResetObject()
        {
            // Move back to start
            transform.position = startPosition;
            transform.rotation = startRotation;

            // Stop any leftover physics movement
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
