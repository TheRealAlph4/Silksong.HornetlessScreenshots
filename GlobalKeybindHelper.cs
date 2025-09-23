using HutongGames.PlayMaker.Actions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Silksong.HornetlessScreenshots
{
    internal class GlobalKeybindHelper : MonoBehaviour
    {
        private readonly HashSet<GameObject> Lights = [];
        private readonly HashSet<GameObject> Vignettes = [];
        public void Update()
        {
            bool toggleAll = Input.GetKeyDown(KeyCode.Alpha0);
            if (toggleAll || Input.GetKeyDown(KeyCode.Alpha1))
            {
                MeshRenderer hornetMeshRenderer = (MeshRenderer)HeroController.instance.GetComponentInParent(typeof(MeshRenderer));
                hornetMeshRenderer.enabled ^= true;
            }
            if (toggleAll || Input.GetKeyDown(KeyCode.Alpha2))
            {
                foreach (GameObject light in GameObject.FindGameObjectsWithTag("HeroLightMain"))
                {
                    Lights.Add(light);
                }
                foreach(GameObject light in Lights)
                {
                    light?.SetActive(!light.activeSelf);
                }
            }
            if (toggleAll || Input.GetKeyDown(KeyCode.Alpha3))
            {
                // status vignette
                GameObject parent = GameObject.Find("In-game");
                parent.transform.GetChild(8).gameObject?.SetActive(false); 
                // hero vignette
                foreach (GameObject vignette in GameObject.FindGameObjectsWithTag("Vignette"))
                {
                    Vignettes.Add(vignette);
                }
                foreach (GameObject vignette in Vignettes)
                {
                    vignette?.SetActive(!vignette.activeSelf);
                }
                // hero effects (e.g. particles)
                GameObject effects = GameObject.Find("Effects");
                effects?.SetActive(!effects.activeSelf);
            }
            if (toggleAll || Input.GetKeyDown(KeyCode.Alpha4))
            {
                HudGlobalHide.IsHidden ^= true;
            }
        }
    }
}
