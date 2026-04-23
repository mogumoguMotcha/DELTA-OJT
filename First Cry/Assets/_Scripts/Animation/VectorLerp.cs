using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Delivery_Room.Script
{
    public class FetusMovement : MonoBehaviour
    {
        public Vector3 targetPosition;  // The target position to move towards
        public float speed = 1.0f;  // The speed of movement (increased for better visibility)
        private bool _isMoving;

        private Vector3 _startPosition; // Variable to store the starting position

        // Reference to the button
        public Button moveButton;

        void Start()
        {
            // Add listener to button click
            moveButton.onClick.AddListener(StartMovement);
        }

        void Update()
        {
            if (_isMoving)
            {
                // Move the fetus along its local Z-axis direction (green arrow)
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

                // Optional: Stop movement when the object is close to the target position
                if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                {
                    _isMoving = false;  // Stop moving
                    Debug.Log("Target position reached.");
                }
            }
        }

        // Function to start the movement when the button is clicked
        public void StartMovement()
        {
            _isMoving = true;
            _startPosition = transform.position;  // Store the starting position here
            Debug.Log("Movement Started");
            Debug.Log("Starting position: " + _startPosition);  // Log the starting position
        }
    }
}