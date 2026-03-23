using UnityEngine;

namespace Delivery_Room.Script
{
    public class SinkWater : MonoBehaviour
    {
        public ParticleSystem waterStream;

        private bool isOn = false;

        public void ToggleWater()
        {
            if (isOn)
            {
                waterStream.Stop();
                isOn = false;
            }
            else
            {
                waterStream.Play();
                isOn = true;
            }
        }
    }
}