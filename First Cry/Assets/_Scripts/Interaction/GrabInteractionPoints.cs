using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // Import XR Interaction Toolkit for XR components

namespace Delivery_Room.Script
{
    public class GrabInteractionPoints : MonoBehaviour
    {
        private XRGrabInteractable _grabInteractable;  // Handle XR grab interactable component
        private bool _hasBeenGrabbed; // Flag to ensure score only increases once per object/button

        // Reference to ScoringManager, assignable in the Inspector
        [SerializeField] private ScoringManager scoringManager;

        void Start()
        {
            // Ensure ScoringManager is assigned
            if (scoringManager == null)
            {
                Debug.LogError("ScoringManager is not assigned in the Inspector.");
            }
            else
            {
                Debug.Log("ScoringManager found and assigned.");
            }

            // Retrieve the XRGrabInteractable component from this GameObject
            _grabInteractable = GetComponent<XRGrabInteractable>();

            if (_grabInteractable == null)
            {
                Debug.LogError("XRGrabInteractable component not found on " + gameObject.name);
            }
            else
            {
                // Register listeners for the grab events
                _grabInteractable.selectEntered.AddListener(OnObjectGrabbed);
                _grabInteractable.selectExited.AddListener(OnObjectReleased);
            }
        }

        // This function will be called when the object is grabbed
        private void OnObjectGrabbed(SelectEnterEventArgs interactor)
        {
            if (!_hasBeenGrabbed && scoringManager != null)
            {
                // Increment points when the object is grabbed for the first time
                scoringManager.CompleteStep();
                _hasBeenGrabbed = true; // Set flag to prevent further score increments for this object
                Debug.Log("Score updated: Object grabbed.");
            }
        }

        // This function will be called when the object is released
        private void OnObjectReleased(SelectExitEventArgs interactor)
        {
            // Reset the grabbed state when the object is released, allowing it to be grabbed again
            _hasBeenGrabbed = false;
            Debug.Log("Object released and ready to be grabbed again.");
        }

        // Clean up event listeners when the object is destroyed
        private void OnDestroy()
        {
            if (_grabInteractable != null)
            {
                _grabInteractable.selectEntered.RemoveListener(OnObjectGrabbed);
                _grabInteractable.selectExited.RemoveListener(OnObjectReleased);
            }
        }
    }
}
