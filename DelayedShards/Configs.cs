using BepInEx.Configuration;
using UnityEngine;

namespace DelayedShards
{
    internal class Configs
    {
        internal static ConfigEntry<KeyCode> SummonEscortConfig;
        internal static ConfigEntry<KeyCode> SummonMinefieldConfig;
        internal static ConfigEntry<bool> DisplayGUIAsPilot;
        internal static ConfigEntry<bool> AlwaysDisplayGUI;

        internal static ConfigEntry<float> GUIPosX;
        internal static ConfigEntry<float> GUIPosY;

        internal static ConfigEntry<bool> enableQueue;

        internal static bool hostHasMod = false;

        internal static void Load(BepinPlugin plugin)
        {
            SummonEscortConfig = plugin.Config.Bind("DelayedShards", "SummonEscort", KeyCode.Alpha2);
            SummonMinefieldConfig = plugin.Config.Bind("DelayedShards", "SummonMinefield", KeyCode.Alpha3);
            DisplayGUIAsPilot = plugin.Config.Bind("DelayedShards", "RecieveNotificationsAsPilot", true);
            AlwaysDisplayGUI = plugin.Config.Bind("DelayedShards", "AlwaysRecieveNotifications", false);

            GUIPosX = plugin.Config.Bind("DelayedShards", "GUIPosX", 0.17f);
            GUIPosY = plugin.Config.Bind("DelayedShards", "GUIPosY", 0.789f);

            enableQueue = plugin.Config.Bind("DelayedShards", "EnableQueue", true);
        }
    }
}
