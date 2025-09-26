using UnityEngine;

namespace HornetlessScreenshots
{
    internal class RemoveLightOnDestroy : MonoBehaviour
    {
        void OnDestroy()
        {
            HornetlessScreenshotsMod.Lights.Remove(this.gameObject);
        }
    }
}
