using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Delivery_Room.Script
{
    public class RadialLoader : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Image radialLoaderImage;     // Must be Type=Filled, Fill Method=Radial
        [SerializeField] private Button[] buttons;            // Optional: One per canvas/panel (can be empty)
        [SerializeField] private Canvas[] canvases;           // Optional: One per button (can be empty)
        [SerializeField] private Canvas[] resultCanvases;     // New array for result canvases to be shown after radial loader

        [Header("Timing Settings")]
        [Tooltip("How long the radial should take to fill (seconds).")]
        [SerializeField] private float loadingDuration = 3.00f;

        [Header("Behavior Settings")]
        [SerializeField] private bool hideClickedButton;

        private Coroutine _loadingRoutine;

        private void Start()
        {
            if (radialLoaderImage == null)
            {
                Debug.LogError("[RadialLoader] Radial Loader Image is not assigned!");
                enabled = false;
                return;
            }

            // Initial UI state
            radialLoaderImage.fillAmount = 0f;
            radialLoaderImage.gameObject.SetActive(false);

            // If you use canvases, start hidden
            SetAllCanvasesActive(false);

            // Wire buttons if provided
            if (buttons != null && canvases != null && buttons.Length == canvases.Length)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    int idx = i;
                    if (buttons[i] != null)
                        buttons[i].onClick.AddListener(() => OnButtonClick(idx));
                }
            }
            else if (buttons != null && buttons.Length > 0)
            {
                Debug.LogWarning("[RadialLoader] Buttons and canvases count mismatch or canvases not set. Button-click routing will be skipped.");
            }
        }

        private void OnButtonClick(int index)
        {
            if (hideClickedButton && buttons != null && index >= 0 && index < buttons.Length && buttons[index] != null)
                buttons[index].gameObject.SetActive(false);

            StartLoading(index);
        }

        /// <summary>
        /// Starts loading and then shows only the canvas at 'index' (if canvases are assigned).
        /// </summary>
        public void StartLoading(int index)
        {
            // Restart loading if already running
            if (_loadingRoutine != null)
                StopCoroutine(_loadingRoutine);

            _loadingRoutine = StartCoroutine(DoRadialLoadThen(() =>
            {
                // Show the result canvas for 3 seconds
                ShowResultCanvas(index);

                // After result canvas disappears, show the corresponding canvas
                StartCoroutine(ShowCanvasAfterResultCanvas(index, 3f));
            }));
        }

        /// <summary>
        /// Parameterless overload to match ButtonHandler. 
        /// If canvases exist, defaults to index 0; otherwise just plays the loader.
        /// </summary>
        public void StartLoading()
        {
            // Restart loading if already running
            if (_loadingRoutine != null)
                StopCoroutine(_loadingRoutine);

            _loadingRoutine = StartCoroutine(DoRadialLoadThen(() =>
            {
                if (canvases != null && canvases.Length > 0)
                    ShowOnlyCanvas(0); // default to first canvas

                // Show the result canvas for 3 seconds
                ShowResultCanvas(0);
                // After result canvas disappears, show the corresponding canvas
                StartCoroutine(ShowCanvasAfterResultCanvas(0, 3f));
            }));
        }

        private IEnumerator DoRadialLoadThen(System.Action onComplete)
        {
            // Prep loader
            radialLoaderImage.gameObject.SetActive(true);
            radialLoaderImage.fillAmount = 0f;

            float t = 0f;
            float duration = Mathf.Max(0.0001f, loadingDuration);

            while (t < duration)
            {
                t += Time.deltaTime;
                radialLoaderImage.fillAmount = Mathf.Clamp01(t / duration);
                yield return null;
            }

            // Complete and hide loader
            radialLoaderImage.fillAmount = 1f;
            radialLoaderImage.gameObject.SetActive(false);

            onComplete?.Invoke();

            _loadingRoutine = null;
        }

        private void SetAllCanvasesActive(bool active)
        {
            if (canvases == null) return;
            foreach (Canvas c in canvases)
                if (c != null) c.gameObject.SetActive(active);
        }

        private void ShowOnlyCanvas(int index)
        {
            if (canvases == null) return;
            for (int i = 0; i < canvases.Length; i++)
                if (canvases[i] != null) canvases[i].gameObject.SetActive(i == index);
        }

        private void ShowResultCanvas(int index)
        {
            // If result canvas is assigned, show the corresponding one based on the button clicked (index)
            if (resultCanvases != null && index >= 0 && index < resultCanvases.Length)
            {
                resultCanvases[index].gameObject.SetActive(true);
            }
        }

        private IEnumerator ShowCanvasAfterResultCanvas(int index, float delay)
        {
            // Wait for the result canvas to be displayed for the given delay
            yield return new WaitForSeconds(delay);

            // Hide the result canvas after the delay
            if (resultCanvases != null && index >= 0 && index < resultCanvases.Length)
                resultCanvases[index].gameObject.SetActive(false);

            // Show the corresponding canvas (after result canvas)
            if (canvases != null && index >= 0 && index < canvases.Length)
                ShowOnlyCanvas(index);
        }
    }
}
