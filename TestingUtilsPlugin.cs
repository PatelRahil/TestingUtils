using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using CoreLib;
using System.Collections;
using UnityEngine;

namespace TestingUtils
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.le4fless.corelib")]
    [BepInProcess("CoreKeeper.exe")]
    public class TestingUtilsPlugin : BasePlugin
    {
        public static ManualLogSource logSource;
        public override void Load()
        {
            // Plugin startup logic
            logSource = Log;
            logSource.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }
    }
}
