using UnityEngine;

// Required for scene management (if you're using a scene exit flow)

namespace Delivery_Room.Script
{
    public class ExitButton : MonoBehaviour
    {
        // This method will be called when the button is clicked
        public void ExitGame()
        {
            // If the game is running in the editor (Unity editor), stop the play mode
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // If the game is built, quit the application
        Application.Quit();
#endif
        }
    }
}
