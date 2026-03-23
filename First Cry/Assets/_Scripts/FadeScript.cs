using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Delivery_Room.Script
{
    public class SceneFadeTransition : MonoBehaviour
    {
        public Image fadeImage;  // Reference to the Image component of the FadePanel
        public float fadeSpeed = 5f;  // Speed of the fade transition (higher value means faster fade)
        public string nextSceneName = "MainScene"; // The scene name to transition to

        private void Start()
        {
            // Ensure the fade panel starts completely black
            fadeImage.color = new Color(0f, 0f, 0f, 1f);  // Black with full opacity

            // Automatically start the fade transition when the scene starts
            StartCoroutine(TransitionToScene());
        }

        // Coroutine to handle the fade transition (no transparent effect)
        private IEnumerator TransitionToScene()
        {
            // Fade out (remain black, no transparency, just keep the screen black)
            yield return new WaitForSeconds(1f);  // Optional delay before transition

            // Load the next scene
            SceneManager.LoadScene(nextSceneName);
        }
    }
}