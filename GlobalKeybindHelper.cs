using System;
using System.Collections.Generic;
using UnityEngine;

namespace HornetlessScreenshots
{
    internal class GlobalKeybindHelper : MonoBehaviour
    {
        public enum Keybind
        {
            All,
            HeroModel,
            HeroLight,
            Vignette,
            HUD,
            FogBlurWind,
            Noclip,
            Freecam,
            Enemies,
            ZoomReset,
            ZoomIn,
            ZoomOut,
            BrightnessReset,
            GeneralBrightnessDown,
            GeneralBrightnessUp,
            HeroLightBrightnessDown,
            HeroLightBrightnessUp,
            Up,
            Down,
            Left,
            Right,
        }
        public readonly Dictionary<Keybind, Func<bool>> Keybinds = new() {
            { Keybind.All, () => Input.GetKeyDown(KeyCode.Alpha0) },
            { Keybind.HeroModel, () => Input.GetKeyDown(KeyCode.Alpha1) },
            { Keybind.HeroLight, () => Input.GetKeyDown(KeyCode.Alpha2) },
            { Keybind.Vignette, () => Input.GetKeyDown(KeyCode.Alpha3) },
            { Keybind.HUD, () => Input.GetKeyDown(KeyCode.Alpha4) },
            { Keybind.FogBlurWind, () => Input.GetKeyDown(KeyCode.Delete) },
            { Keybind.Noclip, () => Input.GetKeyDown(KeyCode.Alpha8) },
            { Keybind.Freecam, () => Input.GetKeyDown(KeyCode.Alpha9) },
            { Keybind.Enemies, () => Input.GetKeyDown(KeyCode.Backspace) },
            { Keybind.ZoomReset, () => Input.GetKeyDown(KeyCode.Alpha5) },
            { Keybind.ZoomIn, () => Input.GetKeyDown(KeyCode.Alpha6) },
            { Keybind.ZoomOut, () => Input.GetKeyDown(KeyCode.Alpha7) },
            { Keybind.BrightnessReset, () => Input.GetKeyDown(KeyCode.Equals) },
            { Keybind.GeneralBrightnessDown, () => Input.GetKeyDown(KeyCode.KeypadMinus) },
            { Keybind.GeneralBrightnessUp, () => Input.GetKeyDown(KeyCode.KeypadPlus) },
            { Keybind.HeroLightBrightnessDown, () => Input.GetKeyDown(KeyCode.KeypadDivide) },
            { Keybind.HeroLightBrightnessUp, () => Input.GetKeyDown(KeyCode.KeypadMultiply) },
            { Keybind.Up, () => Input.GetKey(KeyCode.UpArrow) },
            { Keybind.Down, () => Input.GetKey(KeyCode.DownArrow) },
            { Keybind.Left, () => Input.GetKey(KeyCode.LeftArrow) },
            { Keybind.Right, () => Input.GetKey(KeyCode.RightArrow) },
        };

        public readonly Dictionary<Keybind, Action> Handlers = new() {
            { Keybind.All, HornetlessScreenshotsMod.ToggleAllPressed },
            { Keybind.HeroModel, HornetlessScreenshotsMod.ToggleHeroModelPressed },
            { Keybind.HeroLight, HornetlessScreenshotsMod.ToggleHeroLightPressed },
            { Keybind.Vignette, HornetlessScreenshotsMod.ToggleVignettePressed },
            { Keybind.HUD, HornetlessScreenshotsMod.ToggleHUDPressed },
            { Keybind.FogBlurWind, HornetlessScreenshotsMod.ToggleFogBlurWindPressed },
            { Keybind.Noclip, HornetlessScreenshotsMod.ToggleNoclipPressed },
            { Keybind.Freecam, HornetlessScreenshotsMod.ToggleFreecamPressed },
            { Keybind.Enemies, HornetlessScreenshotsMod.ToggleEnemiesPressed },
            { Keybind.ZoomReset, HornetlessScreenshotsMod.ResetZoom },
            { Keybind.ZoomIn, () => HornetlessScreenshotsMod.IncreaseZoom(0.05f) },
            { Keybind.ZoomOut, () => HornetlessScreenshotsMod.IncreaseZoom(-0.05f) },
            { Keybind.BrightnessReset, HornetlessScreenshotsMod.ResetAllBrightnesses },
            { Keybind.GeneralBrightnessDown, () => HornetlessScreenshotsMod.IncreaseGeneralBrightness(-0.25f) },
            { Keybind.GeneralBrightnessUp, () => HornetlessScreenshotsMod.IncreaseGeneralBrightness(0.25f) },
            { Keybind.HeroLightBrightnessDown, () => HornetlessScreenshotsMod.IncreaseHeroLightBrightness(-1f) },
            { Keybind.HeroLightBrightnessUp, () => HornetlessScreenshotsMod.IncreaseHeroLightBrightness(1f) },
            { Keybind.Up, () => HornetlessScreenshotsMod.ArrowKeyPressed(0f, 1f) },
            { Keybind.Down, () => HornetlessScreenshotsMod.ArrowKeyPressed(0f, -1f) },
            { Keybind.Left, () => HornetlessScreenshotsMod.ArrowKeyPressed(-1f, 0f) },
            { Keybind.Right, () => HornetlessScreenshotsMod.ArrowKeyPressed(1f, 0f) },
        };

        public static bool IsShiftHeld
        {
            get
            {
                return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            }
        }

        public void Update()
        {
            foreach (KeyValuePair<Keybind, Func<bool>> pair in Keybinds)
            {
                Keybind bind = pair.Key;
                Func<bool> condition = pair.Value;
                if(condition())
                {
                    try
                    {
                        Handlers[bind]?.Invoke();
                    }
                    catch (Exception)
                    {
                    }
                }
            }


        }
    }
}
