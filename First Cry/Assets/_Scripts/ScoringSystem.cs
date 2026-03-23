using TMPro;
using UnityEngine;

namespace Delivery_Room.Script
{
    public class ScoringManager : MonoBehaviour
    {
        public int totalSteps = 3; // Total steps in the scenario
        private int _currentStep; // Tracks the current completed steps
        public TextMeshProUGUI scoreText; // Reference to the UI Text (TMP) for score
        public TextMeshProUGUI accomplishedText; // Reference to the UI Text (TMP) for accomplished
        public TextMeshProUGUI passFailText; // Reference to the UI Text (TMP) for pass/fail

        void Start()
        {
            UpdateScoreText(); // Ensure the score is displayed at the start
        }

        // This method is called when a step is completed
        public void CompleteStep()
        {
            // If the current step is not beyond the total steps
            if (_currentStep < totalSteps)
            {
                _currentStep++; // Increment the completed steps
                UpdateScoreText(); // Update the UI with the new score
            }

            // Check if all steps are completed
            if (_currentStep == totalSteps)
            {
                passFailText.text = "Pass"; // Mark as pass if all steps are completed
                Debug.Log("Scenario Completed! Pass.");
            }
            else
            {
                passFailText.text = "Fail"; // Mark as fail if any step is missed
                Debug.Log("A step was missed! Fail.");
            }
        }

        // Updates the score text on the UI
        private void UpdateScoreText()
        {
            // Format score as "1/3"
            if (scoreText != null)
            {
                scoreText.text = $"{_currentStep}/{totalSteps}";
            }
            if (accomplishedText != null)
            {
                accomplishedText.text = $"Accomplished: {_currentStep}/{totalSteps}"; // Update accomplished text
            }
        }

        // This method allows resetting the score (useful for restarting the game)
        public void ResetScore()
        {
            _currentStep = 0;
            passFailText.text = ""; // Clear pass/fail text
            UpdateScoreText();
            Debug.Log("Score has been reset.");
        }
    }
}
