using UnityEngine;

namespace HornetlessScreenshots
{
    internal class RemoveVignetteOnDestroy : MonoBehaviour
    {
        void OnDestroy()
        {
            HornetlessScreenshotsMod.Vignettes.Remove(this.gameObject);
        }
    }
}
