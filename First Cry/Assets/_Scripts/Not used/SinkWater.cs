/// <summary>
/// UNECESSARY SCRIPT: This script is not currently used in the project,
///                     but it can be used to control a water stream particle system for a sink.
///                     It allows toggling the water on and off, which can be useful for interactive elements in the game.
/// </summary>

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