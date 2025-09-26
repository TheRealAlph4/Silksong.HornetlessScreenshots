using BepInEx;
using System.Collections.Generic;
using UnityEngine;

namespace HornetlessScreenshots;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class HornetlessScreenshotsMod : BaseUnityPlugin
{
    public static readonly HashSet<GameObject> Lights = [];
    public static readonly HashSet<GameObject> Vignettes = [];

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(new GameObject("update_object", [typeof(GlobalKeybindHelper)]));
    }

    public static void ToggleAllPressed()
    {
        ToggleHeroModelPressed();
        ToggleHeroLightPressed();
        ToggleVignettePressed();
        ToggleHUDPressed();
    }

    public static void ToggleHeroModelPressed()
    {
        MeshRenderer hornetMeshRenderer = (MeshRenderer)HeroController.instance.GetComponentInParent(typeof(MeshRenderer));
        hornetMeshRenderer.enabled ^= true;
    }

    public static void ToggleHeroLightPressed()
    {
        foreach (GameObject light in GameObject.FindGameObjectsWithTag("HeroLightMain"))
        {
            if (!Lights.Contains(light))
            {
                light.AddComponent<RemoveLightOnDestroy>();
            }
            Lights.Add(light);
        }
        foreach (GameObject light in Lights)
        {
            light?.SetActive(!light.activeSelf);
        }
    }

    public static void ToggleVignettePressed()
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

    public static void ToggleHUDPressed()
    {
        HudGlobalHide.IsHidden ^= true;
    }
}