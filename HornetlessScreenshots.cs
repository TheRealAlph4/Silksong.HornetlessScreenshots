using BepInEx;
using UnityEngine;

namespace Silksong.HornetlessScreenshots;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Silksong_HornetlessScreenshots : BaseUnityPlugin
{
    private void Awake()
    {
        GameObject.DontDestroyOnLoad(new GameObject("update_object", [typeof(GlobalKeybindHelper)]));
    }
}