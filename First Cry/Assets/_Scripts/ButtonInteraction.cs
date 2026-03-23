using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
// Import XR Toolkit for XR components
using UnityEngine.XR.Interaction.Toolkit.Interactables; // For interaction events

namespace Delivery_Room.Script
{
    public class ButtonInteraction : MonoBehaviour
    {
        private ScoringManager _scoringManager;
        private XRGrabInteractable _grabInteractable; // Updated to the correct XRGrabInteractable type

        [Obsolete("Obsolete")]
        void Start()
        {
            // Find the ScoringManager script in the scene
            _scoringManager = FindObjectOfType<ScoringManager>();

            // Find the XRGrabInteractable component if this object can be grabbed
            _grabInteractable = GetComponent<XRGrabInteractable>();

            if (_grabInteractable != null)
            {
                _grabInteractable.selectEntered.AddListener(OnObjectGrabbed); // Add listener for grab events
            }
            else
            {
                Debug.LogWarning("XRGrabInteractable not found on " + gameObject.name);
            }
        }

        private void OnObjectGrabbed(SelectEnterEventArgs arg0)
        {
            throw new NotImplementedException();
        }

        // This function will be called when the button is clicked via UI interaction
        public void OnButtonClick()
        {
            if (_scoringManager != null)
            {
                _scoringManager.CompleteStep(); // Increment the score
            }

            // Additional logic like enabling objects or other actions
            GameObject radialCar = GameObject.Find("RadialCar"); // Find the object by name
            if (radialCar != null)
            {
                radialCar.SetActive(true); // Activate the RadialCar GameObject
            }
        }

        // This function will be called when the button is grabbed

        // Clean up the event listeners when the object is destroyed
        private void OnDestroy()
        {
            if (_grabInteractable != null)
            {
                _grabInteractable.selectEntered.RemoveListener(OnObjectGrabbed); // Remove listener on destruction
            }
        }
    }
}
