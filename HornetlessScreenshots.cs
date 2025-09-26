using BepInEx;
using System.Collections.Generic;
using UnityEngine;

namespace HornetlessScreenshots;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class HornetlessScreenshotsMod : BaseUnityPlugin
{
    public static readonly HashSet<GameObject> Lights = [];
    public static readonly HashSet<GameObject> Vignettes = [];

    public static bool AllVisible = true;
    public static bool HeroModelVisible = true;
    public static bool HeroLightVisible = true;
    public static bool VignetteVisible = true;
    public static bool HUDVisible = true;

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(new GameObject("update_object", [typeof(GlobalKeybindHelper)]));
    }

    public static void ToggleAllPressed()
    {
        SetAllVisible(!AllVisible);
    }

    public static void SetAllVisible(bool visible)
    {
        SetHeroModelVisible(visible);
        SetHeroLightVisible(visible);
        SetVignetteVisible(visible);
        SetHUDVisible(visible);
        AllVisible = visible;
    }

    public static void ToggleHeroModelPressed()
    {
        SetHeroModelVisible(!HeroModelVisible);
    }

    public static void SetHeroModelVisible(bool visible)
    {
        MeshRenderer hornetMeshRenderer = (MeshRenderer)HeroController.instance.GetComponentInParent(typeof(MeshRenderer));
        hornetMeshRenderer.enabled = visible;
        HeroModelVisible = visible;
    }

    public static void ToggleHeroLightPressed()
    {
        SetHeroLightVisible(!HeroLightVisible);
    }

    public static void SetHeroLightVisible(bool visible)
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
            light?.SetActive(visible);
        }
        HeroLightVisible = visible;
    }

    public static void ToggleVignettePressed()
    {
        SetVignetteVisible(!VignetteVisible);
    }

    public static void SetVignetteVisible(bool visible)
    {
        // status vignette
        GameObject parent = GameObject.Find("In-game");
        parent.transform.GetChild(8).gameObject?.SetActive(false);
        // hero vignette
        foreach (GameObject vignette in GameObject.FindGameObjectsWithTag("Vignette"))
        {
            if (!Vignettes.Contains(vignette))
            {
                vignette.AddComponent<RemoveVignetteOnDestroy>();
            }
            Vignettes.Add(vignette);
        }
        foreach (GameObject vignette in Vignettes)
        {
            vignette?.SetActive(visible);
        }
        // hero effects (e.g. particles)
        GameObject effects = GameObject.Find("Effects");
        effects?.SetActive(visible);
        VignetteVisible = visible;
    }

    public static void ToggleHUDPressed()
    {
        SetHUDVisible(!HUDVisible);
    }

    public static void SetHUDVisible(bool visible)
    {
        HudGlobalHide.IsHidden = !visible;
        HUDVisible = visible;
    }
}