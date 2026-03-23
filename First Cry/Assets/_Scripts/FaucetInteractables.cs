using UnityEngine;

namespace Delivery_Room.Script
{
    public class FaucetInteraction : MonoBehaviour
    {
        private ScoringManager _scoringManager;

        // Reference to the water particle system
        [SerializeField] private ParticleSystem waterParticleSystem;  // Drag your water particle system here in the Inspector

        private bool _isWaterOn; // Track the state of the water (on/off)

        // Start is called before the first frame update
        void Start()
        {
            // Find the ScoringManager in the scene using the updated method
            _scoringManager = UnityEngine.Object.FindFirstObjectByType<ScoringManager>();
            if (_scoringManager == null)
            {
                Debug.LogError("ScoringManager not found in the scene!");
            }
        }

        // This function is called when the collider of the faucet is triggered
        private void OnTriggerEnter(Collider other)
        {
            // Check if the object has the tag "Interact"
            if (other.CompareTag("Interact"))
            {
                Debug.Log("Faucet interaction detected!");  // Debugging log to confirm interaction

                // If the water isn't already on, toggle it on and update the score
                if (!_isWaterOn)
                {
                    ToggleWater(); // Turn on the water
                    if (_scoringManager != null)
                    {
                        _scoringManager.CompleteStep(); // Automatically increment the score when triggered
                        Debug.Log("Faucet interacted with! Score updated.");  // Confirm score update
                    }
                }
                else
                {
                    Debug.Log("Water is already on.");  // Debugging log if water is already on
                }
            }
        }

        // Toggle the water particle system (start/stop the water)
        private void ToggleWater()
        {
            if (_isWaterOn)
            {
                waterParticleSystem.Stop(); // Stop the water particle system
                _isWaterOn = false; // Update state
                Debug.Log("Water stopped.");  // Debugging log when water stops
            }
            else
            {
                waterParticleSystem.Play(); // Start the water particle system
                _isWaterOn = true; // Update state
                Debug.Log("Water started.");  // Debugging log when water starts
            }
        }

        // Clean up event listeners when the object is destroyed
    }
}
