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
            Noclip,
            Freecam,
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
            { Keybind.Noclip, () => Input.GetKeyDown(KeyCode.Alpha8) },
            { Keybind.Freecam, () => Input.GetKeyDown(KeyCode.Alpha9) },
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
            { Keybind.Noclip, HornetlessScreenshotsMod.ToggleNoclipPressed },
            { Keybind.Freecam, HornetlessScreenshotsMod.ToggleFreecamPressed },
            { Keybind.Up, () => HornetlessScreenshotsMod.ArrowKeyPressed(0, 1) },
            { Keybind.Down, () => HornetlessScreenshotsMod.ArrowKeyPressed(0, -1) },
            { Keybind.Left, () => HornetlessScreenshotsMod.ArrowKeyPressed(-1, 0) },
            { Keybind.Right, () => HornetlessScreenshotsMod.ArrowKeyPressed(1, 0) },
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
