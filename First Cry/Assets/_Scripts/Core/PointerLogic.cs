using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

#if UNITY_XR_INTERACTION_TOOLKIT
using UnityEngine.XR.Interaction.Toolkit;
#endif

namespace Delivery_Room.Script
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))] // OK for both modes; leave Kinematic true
    public class TriggerRadialLoader : MonoBehaviour
    {
        [FormerlySerializedAs("useXRIHover")]
        [Header("Mode")]
        [Tooltip("Off = use physics Trigger (current). On = use XR Interaction Toolkit hover events (Poke/Near/Far).")]
        public bool useXriHover;

        [Header("UI")]
        public Image radialImage;                 // UI Image set to Filled/Radial
        public GameObject radialRoot;             // Optional: parent canvas to show/hide
        public GameObject buttonToShow;           // Reference to the UI button that will appear after completion
        public float holdDuration = 1.5f;         // Seconds to reach full
        public float resetSpeed = 2f;             // How fast it drains when not touching

        [Header("Filtering (Trigger mode)")]
        public LayerMask triggerAgainstLayers = ~0;   // Which layers can trigger
        public string requiredTag = "";               // Optional: leave empty to ignore tag

#if UNITY_XR_INTERACTION_TOOLKIT
        [Header("Which Interactors Count (XRI mode)")]
        public bool allowPoke = true;     // XRPokeInteractor (finger)
        public bool allowNear = true;     // XRDirectInteractor (hand proximity)
        public bool allowFar  = false;    // XRRayInteractor (laser)
#endif

        [Header("Events")]
        public UnityEvent onStartFilling;
        public UnityEvent onCompleted;
        public UnityEvent onCancel;

        // --- private state ---
        float _currentFill;
        bool _active;                 // true while 'hovering/inside' depending on mode
        bool _hasStarted;
        bool _completedThisContact;

        Collider _col;
        Rigidbody _rb;

#if UNITY_XR_INTERACTION_TOOLKIT
        XRSimpleInteractable _interactable;
        readonly HashSet<IXRInteractor> _hovering = new HashSet<IXRInteractor>();
#endif

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _col = GetComponent<Collider>();

            // Base setup
            _rb.isKinematic = true;

            if (radialImage)
            {
                radialImage.type = Image.Type.Filled;
                radialImage.fillMethod = Image.FillMethod.Radial360;
                radialImage.fillAmount = 0f;
            }
            if (radialRoot)
            {
                radialRoot.SetActive(false);  // Initially hide the radial loader UI
            }

            if (buttonToShow)
            {
                buttonToShow.SetActive(false);  // Hide the button initially
            }

            // Configure by mode
            if (useXriHover)
            {
#if UNITY_XR_INTERACTION_TOOLKIT
                // XRI mode: non-trigger collider works best for poke/near
                _col.isTrigger = false;

                // Add or get XRSimpleInteractable
                _interactable = GetComponent<XRSimpleInteractable>();
                if (_interactable == null)
                    _interactable = gameObject.AddComponent<XRSimpleInteractable>();
#else
                Debug.LogWarning("TriggerRadialLoader: useXRIHover is ON but XR Interaction Toolkit is not present.");
#endif
            }
            else
            {
                // Trigger mode (your current working behavior)
                _col.isTrigger = true;
            }
        }

        void OnEnable()
        {
#if UNITY_XR_INTERACTION_TOOLKIT
            if (useXRIHover && _interactable != null)
            {
                _interactable.hoverEntered.AddListener(OnHoverEntered);
                _interactable.hoverExited.AddListener(OnHoverExited);
            }
#endif
        }

        void OnDisable()
        {
#if UNITY_XR_INTERACTION_TOOLKIT
            if (useXRIHover && _interactable != null)
            {
                _interactable.hoverEntered.RemoveListener(OnHoverEntered);
                _interactable.hoverExited.RemoveListener(OnHoverExited);
            }
#endif
        }

        void Update()
        {
            if (_active)
            {
                if (!_hasStarted && _currentFill <= 0f)
                {
                    _hasStarted = true;
                    _completedThisContact = false;
                    if (radialRoot) radialRoot.SetActive(true);  // Show radial loader canvas once the action starts
                    onStartFilling?.Invoke();
                }

                if (!_completedThisContact)
                {
                    _currentFill += Time.deltaTime / Mathf.Max(0.01f, holdDuration);
                    if (_currentFill >= 1f)
                    {
                        _currentFill = 1f;
                        _completedThisContact = true;
                        onCompleted?.Invoke();
                        ShowButtonAfterCompletion();  // Show the button after the radial loader is complete
                        HideCanvasAfterCompletion();  // Hide the canvas once the radial loader is complete
                    }
                }
            }
            else
            {
                if (_hasStarted && !_completedThisContact)
                    onCancel?.Invoke();

                _hasStarted = false;
                _currentFill = Mathf.MoveTowards(_currentFill, 0f, Time.deltaTime * resetSpeed);

                if (_currentFill <= 0.0001f && radialRoot && radialRoot.activeSelf)
                    radialRoot.SetActive(false);  // Hide canvas when reset
            }

            if (radialImage) radialImage.fillAmount = _currentFill;
        }

        void OnTriggerEnter(Collider other)
        {
            if (useXriHover) return; // ignore in XRI mode
            if (!IsValid(other)) return;
            _active = true;
        }

        void OnTriggerExit(Collider other)
        {
            if (useXriHover) return; // ignore in XRI mode
            if (!IsValid(other)) return;
            _active = false;
            _completedThisContact = false; // allow re-trigger next time
        }

        bool IsValid(Collider other)
        {
            if (((1 << other.gameObject.layer) & triggerAgainstLayers) == 0)
                return false;
            if (!string.IsNullOrEmpty(requiredTag) && !other.CompareTag(requiredTag))
                return false;
            return true;
        }

#if UNITY_XR_INTERACTION_TOOLKIT
        void OnHoverEntered(HoverEnterEventArgs args)
        {
            if (!useXRIHover) return;
            if (IsAllowedInteractor(args.interactorObject))
            {
                _hovering.Add(args.interactorObject);
                _active = _hovering.Count > 0;
            }
        }

        void OnHoverExited(HoverExitEventArgs args)
        {
            if (!useXRIHover) return;
            _hovering.Remove(args.interactorObject);
            _active = _hovering.Count > 0;
            if (!_active) _completedThisContact = false;
        }

        bool IsAllowedInteractor(IXRInteractor interactor)
        {
            if (interactor is XRPokeInteractor)   return allowPoke;
            if (interactor is XRDirectInteractor) return allowNear;
            if (interactor is XRRayInteractor)    return allowFar;
            return false;
        }
#endif

        // Function to show the button after radial loader completion
        private void ShowButtonAfterCompletion()
        {
            if (buttonToShow != null)
            {
                buttonToShow.SetActive(true);  // Show the button when radial loader completes
            }
        }

        // New function to hide the radial loader canvas once completed
        private void HideCanvasAfterCompletion()
        {
            if (radialRoot != null && radialRoot.activeSelf)
            {
                radialRoot.SetActive(false);  // Hide the radial loader canvas after completion
            }
        }

        // Optional: call from UnityEvent to hard reset
        public void ResetFillImmediate()
        {
            _active = false;
            _hasStarted = false;
            _completedThisContact = false;
            _currentFill = 0f;
            if (radialImage) radialImage.fillAmount = 0f;
            if (radialRoot) radialRoot.SetActive(false);
            if (buttonToShow) buttonToShow.SetActive(false);  // Optionally hide the button during reset

#if UNITY_XR_INTERACTION_TOOLKIT
            if (useXRIHover) _hovering.Clear();
#endif
        }
    }
}
