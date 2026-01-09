using BepInEx;
using Mono.Security.Authenticode;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HornetlessScreenshots;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class HornetlessScreenshotsMod : BaseUnityPlugin
{
    public static readonly HashSet<GameObject> Lights = [];
    public static readonly HashSet<GameObject> Vignettes = [];
    public static readonly HashSet<GameObject> FogBlurWind = [];

    public static bool AllVisible = true;
    public static bool HeroModelVisible = true;
    public static bool HeroLightVisible = true;
    public static bool VignetteVisible = true;
    public static bool HUDVisible = true;
    public static bool EnemiesEnabled = true;
    public static bool IsCustomBrightness = false;
    public static bool FogBlurWindVisible = true;

    public static bool IsFreecam = false;
    public static bool IsNoclip = false;

    public static float PreviousBrightness = 1.0f;
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
        HeroController.instance.playerData.isInvincible = isFreecam;
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
        HeroController.instance.playerData.isInvincible = isNoclip;
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

    public static void ResetZoom()
    {
        GameCameras.instance.tk2dCam.ZoomFactor = 1;
    }

    public static void IncreaseZoom(float amount)
    {
        GameCameras.instance.tk2dCam.ZoomFactor += amount;
    }

    public static void ToggleEnemiesPressed()
    {
        SetEnemiesEnabled(!EnemiesEnabled);
    }

    public static void ResetAllBrightnesses()
    {
        if (IsCustomBrightness)
        {
            GameCameras.instance.brightnessEffect.SetBrightness(PreviousBrightness);
            IsCustomBrightness = false;
        }
        foreach (GameObject light in Lights)
        {
            if (light == null) continue;
            light.transform.localScale = new Vector3(3, 3, 1);
        }
    }

    public static void IncreaseGeneralBrightness(float amount)
    {
        float brightness = GameCameras.instance.brightnessEffect._Brightness;
        float shiftHeld = GlobalKeybindHelper.IsShiftHeld ? 0.2f : 1f;
        float actualChange = shiftHeld * amount;
        GameCameras.instance.brightnessEffect.SetBrightness(Math.Max(brightness + actualChange, 0));
        IsCustomBrightness = true;
    }

    public static void IncreaseHeroLightBrightness(float amount)
    {
        float shiftHeld = GlobalKeybindHelper.IsShiftHeld ? 0.2f : 1f;
        float actualChange = shiftHeld * amount;

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
            if (light == null) continue;
            Vector3 localScale = light.transform.localScale;
            localScale.x = Math.Max(localScale.x + actualChange, 0f);
            localScale.y = Math.Max(localScale.y + actualChange, 0f);
            light.transform.localScale = localScale;
        }
    }

    public static void SetEnemiesEnabled(bool enabled)
    {
        HealthManager[] healthManagers = Resources.FindObjectsOfTypeAll<HealthManager>();
        foreach (HealthManager healthManager in healthManagers)
        {
            healthManager.gameObject.SetActive(enabled);
        }
        EnemiesEnabled = enabled;
    }

    public static void ToggleFogBlurWindPressed()
    {
        SetFogBlurWindEnabled(!FogBlurWindVisible);
    }

    public static void SetFogBlurWindEnabled(bool enabled)
    {
        GameObject blurPlane = GameObject.Find("BlurPlane");
        GameObject particles = GameObject.Find("SceneParticlesController");
        GameObject dustStorm = GameObject.Find("Dust Storm Manager");
        GameObject dustParticles = GameObject.Find("dust_particle_set");
        GameObject transitionDustParticles = GameObject.Find("blown_sand_tiled_set");
        FogBlurWind.Add(blurPlane);
        FogBlurWind.Add(particles);
        FogBlurWind.Add(dustStorm);
        FogBlurWind.Add(dustParticles);
        FogBlurWind.Add(transitionDustParticles);
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj == null || !obj.activeInHierarchy) continue;
            string name = obj.name.ToLower();
            if (name.Contains("fog") || name.Contains("dust") || name.Contains("particles"))
            {   
                FogBlurWind.Add(obj);
            }
        }
        foreach (GameObject obj in FogBlurWind)
        {
            obj?.SetActive(enabled);
        }
        FogBlurWindVisible = enabled;
    }
}