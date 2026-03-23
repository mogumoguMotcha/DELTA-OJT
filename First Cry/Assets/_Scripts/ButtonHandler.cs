using UnityEngine;
using UnityEngine.UI;

namespace Delivery_Room.Script
{
    public class ButtonHandler : MonoBehaviour
    {
        [SerializeField] private Button assessButton;       // Assign in Inspector
        [SerializeField] private RadialLoader radialLoader; // Assign the component directly

        private void Start()
        {
            if (assessButton == null)
            {
                Debug.LogError("[ButtonHandler] Assess Button is not assigned!");
                enabled = false;
                return;
            }

            if (radialLoader == null)
            {
                Debug.LogError("[ButtonHandler] RadialLoader component is not assigned!");
                enabled = false;
                return;
            }

            // Button listener
            assessButton.onClick.AddListener(OnAssessButtonClick);

            // Ensure loader object is initially hidden
            var loaderGo = radialLoader.gameObject;
            if (loaderGo != null) loaderGo.SetActive(false);
        }

        private void OnAssessButtonClick()
        {
            if (radialLoader == null)
            {
                Debug.LogError("[ButtonHandler] Cannot start loading: RadialLoader is missing!");
                return;
            }

            // Hide the Assess button
            assessButton.gameObject.SetActive(false);

            // Activate loader object then start loading
            radialLoader.gameObject.SetActive(true);

            // Use the parameterless overload (defaults to first canvas if any)
            radialLoader.StartLoading();

            Debug.Log("[ButtonHandler] Assess button clicked!");
        }
    }
}