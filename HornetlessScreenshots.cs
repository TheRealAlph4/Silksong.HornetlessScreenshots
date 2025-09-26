using BepInEx;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public static bool IsFreecam = false;
    public static bool IsNoclip = false;

    public static float ForcedHeroX = 0;
    public static float ForcedHeroY = 0;

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(new GameObject("HornetlessScreenshots_GlobalKeybindHelper", [typeof(GlobalKeybindHelper)]));
        GameObject.DontDestroyOnLoad(new GameObject("HornetlessScreenshots_NoclipHelper", [typeof(NoclipHelper)]));
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

    public static void ArrowKeyPressed(float dirX, float dirY)
    {
        if (IsNoclip)
        {
            MoveHero(dirX, dirY);
        }
        if (IsFreecam)
        {
            MoveCameraTarget(dirX, dirY);
        }
    }

    public static void ToggleFreecamPressed()
    {
        SetFreecamEnabled(!IsFreecam);
    }

    public static void SetFreecamEnabled(bool isFreecam)
    {
        GameObject cameraTargetGO = GameObject.FindGameObjectWithTag("CameraTarget");
        if (cameraTargetGO == null)
        {
            return;
        }
        CameraTarget target = cameraTargetGO.GetComponent<CameraTarget>();
        target.enabled = !isFreecam;
        HeroController.instance.enabled = !isFreecam;
        SetAllVisible(!isFreecam);
        if (isFreecam)
        {
            SetNoclipEnabled(false);
        }
        IsFreecam = isFreecam;
    }

    public static void MoveCameraTarget(float dirX, float dirY)
    {
        if(!IsFreecam)
        {
            return;
        }
        GameObject cameraTargetGO = GameObject.FindGameObjectWithTag("CameraTarget");
        if (cameraTargetGO == null)
        {
            return;
        }
        float mult = GlobalKeybindHelper.IsShiftHeld ? 0.05f : 0.5f;
        float x = mult * dirX;
        float y = mult * dirY;
        cameraTargetGO.transform.SetPositionX(cameraTargetGO.transform.GetPositionX() + x);
        cameraTargetGO.transform.SetPositionY(cameraTargetGO.transform.GetPositionY() + y);
    }

    public static void ToggleNoclipPressed()
    {
        SetNoclipEnabled(!IsNoclip);
    }

    public static void SetNoclipEnabled(bool isNoclip)
    {
        if (HeroController.instance == null)
        {
            return;
        }

        if (isNoclip)
        {
            SetFreecamEnabled(false);
            ForcedHeroX = HeroController.instance.transform.GetPositionX();
            ForcedHeroY = HeroController.instance.transform.GetPositionY();
            SetAllVisible(true);
        }
        HeroController.instance.takeNoDamage = isNoclip;
        IsNoclip = isNoclip;
    }

    public static void MoveHero(float dirX, float dirY)
    {
        if (!IsNoclip)
        {
            return;
        }
        float mult = GlobalKeybindHelper.IsShiftHeld ? 0.05f : 0.5f;
        float x = mult * dirX;
        float y = mult * dirY;
        ForcedHeroX += x;
        ForcedHeroY += y;
    }
}