using UnityEngine;
using UnityEngine.UI; // For accessing UI components like Image

namespace Delivery_Room.Script
{
    public class BabyMovement : MonoBehaviour
    {
        public Transform colliderDefaultPlace; // Start position (ColliderDefaultPlace)
        public Transform colliderSecondPlace;  // End position (ColliderSecondPlace)
        public Transform colliderThirdPlace;   // New third position (ThirdCollider)
        public float moveSpeed = 1.0f;         // Speed of the movement
        private bool _isMoving;                // To track if the baby is moving
        private bool _hasReachedSecondPlace;   // To track if the baby reached the second place
        private bool _hasReachedThirdPlace;    // To track if the baby reached the third place
        private Vector3 _targetPosition;       // Target position for the baby

        // Reference to the Radial Loader (UI Image)
        public Image radialLoader;

        // UI elements to enable when the baby reaches the final position
        public GameObject finalCanvas;   // Canvas to be enabled
        public Button finalButton;       // Button to be enabled

        void Start()
        {
            // Initially set the target to the first position (ColliderDefaultPlace)
            _targetPosition = colliderDefaultPlace.position;

            // Initialize the radial loader (if assigned)
            if (radialLoader != null)
            {
                radialLoader.fillAmount = 0f; // Start with an empty loader (optional)
            }

            // Initially disable the Canvas and Button (set inactive)
            if (finalCanvas != null) finalCanvas.SetActive(false);
            if (finalButton != null) finalButton.gameObject.SetActive(false);
        }

        void Update()
        {
            if (_isMoving)
            {
                // Calculate distance to target
                float distanceToTarget = Vector3.Distance(transform.position, _targetPosition);

                // Gradually slow down as the baby gets closer (realistic effect)
                float slowdownFactor = Mathf.Clamp(distanceToTarget / 2, 0.1f, 1.0f); // Slow down as it gets closer
                float step = moveSpeed * slowdownFactor * Time.deltaTime; // Adjust step based on slowdown factor

                // Move towards the target position smoothly
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);

                // Check if the baby has reached the target position
                if (transform.position == _targetPosition)
                {
                    // Stop the movement when the baby reaches the third place
                    _isMoving = false;

                    // Mark as reached second or third place, preventing toggle back
                    if (!_hasReachedSecondPlace && _targetPosition == colliderSecondPlace.position)
                    {
                        _hasReachedSecondPlace = true;
                        Debug.Log("Baby has reached the second place and stopped.");
                    }

                    if (!_hasReachedThirdPlace && _targetPosition == colliderThirdPlace.position)
                    {
                        _hasReachedThirdPlace = true;
                        Debug.Log("Baby has reached the third place and stopped.");
                    }

                    // Reset radial loader to full (optional)
                    if (radialLoader != null)
                    {
                        radialLoader.fillAmount = 1f; // Full loader after reaching the target
                    }
                }
            }
        }

        // Method to trigger movement when the button is pressed
        public void StartMoving()
        {
            // If the baby hasn't already reached the second or third place, start moving
            if (!_hasReachedSecondPlace)
            {
                _targetPosition = colliderSecondPlace.position; // Set the target position to the second place
                _isMoving = true;  // Start moving
            }
            else if (!_hasReachedThirdPlace)
            {
                _targetPosition = colliderThirdPlace.position; // Set the target position to the third place
                _isMoving = true;  // Start moving
            }
        }

        // Trigger to stop the movement and enable UI elements when the baby reaches the second or third collider
        private void OnTriggerEnter(Collider other)
        {
            if (other != null && other.CompareTag("SecondCollider"))
            {
                _isMoving = false;  // Stop the movement
                Debug.Log("Baby reached the second place and movement has stopped.");

                // Enable the final Canvas and Button when the baby reaches the second place
                if (finalCanvas != null) finalCanvas.SetActive(true); // Set the canvas to active
                if (finalButton != null) finalButton.gameObject.SetActive(true); // Set the button to active
            }

            if (other && other.CompareTag("ThirdCollider"))
            {
                _isMoving = false;  // Stop the movement
                Debug.Log("Baby reached the third place and movement has stopped.");

                // Enable the final Canvas and Button when the baby reaches the third place
                if (finalCanvas != null) finalCanvas.SetActive(true); // Set the canvas to active
                if (finalButton != null) finalButton.gameObject.SetActive(true); // Set the button to active
            }
        }
    }
}
