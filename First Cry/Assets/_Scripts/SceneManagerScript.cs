using UnityEngine;
using UnityEngine.SceneManagement;

namespace Delivery_Room.Script
{
    public class SceneManagerScript : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            // ?? make sure this matches EXACTLY the scene name
        }
    }
}
