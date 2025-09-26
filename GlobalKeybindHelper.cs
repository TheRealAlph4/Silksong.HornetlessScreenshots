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
        }
        public readonly Dictionary<Keybind, KeyCode> Keybinds = new() {
            { Keybind.All, KeyCode.Alpha0 },
            { Keybind.HeroModel, KeyCode.Alpha1 },
            { Keybind.HeroLight, KeyCode.Alpha2 },
            { Keybind.Vignette, KeyCode.Alpha3 },
            { Keybind.HUD, KeyCode.Alpha4 },
        };
        public readonly Dictionary<Keybind, Action> Handlers = new() {
            { Keybind.All, HornetlessScreenshotsMod.ToggleAllPressed },
            { Keybind.HeroModel, HornetlessScreenshotsMod.ToggleHeroModelPressed },
            { Keybind.HeroLight, HornetlessScreenshotsMod.ToggleHeroLightPressed },
            { Keybind.Vignette, HornetlessScreenshotsMod.ToggleVignettePressed },
            { Keybind.HUD, HornetlessScreenshotsMod.ToggleHUDPressed },
        };

        public void Update()
        {
            foreach (KeyValuePair<Keybind, KeyCode> pair in Keybinds)
            {
                Keybind bind = pair.Key;
                KeyCode button = pair.Value;
                if(Input.GetKeyDown(button))
                {
                    Handlers[bind]?.Invoke();
                }
            }
        }
    }
}
