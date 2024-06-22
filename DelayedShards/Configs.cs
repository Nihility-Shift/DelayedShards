using BepInEx.Configuration;
using UnityEngine;

namespace DelayedShards
{
    internal class Configs
    {
        internal static ConfigEntry<KeyCode> SummonEscortConfig;
        internal static ConfigEntry<KeyCode> SummonMinefieldConfig;
        internal static ConfigEntry<bool> RecieveNotificationsAsPilot;
        internal static ConfigEntry<bool> AlwaysRecieveNotifications;

        internal static void Load(BepinPlugin plugin)
        {
            SummonEscortConfig = plugin.Config.Bind("DelayedShards", "SummonEscort", KeyCode.Alpha2);
            SummonMinefieldConfig = plugin.Config.Bind("DelayedShards", "SummonMinefield", KeyCode.Alpha3);
            RecieveNotificationsAsPilot = plugin.Config.Bind("DelayedShards", "RecieveNotificationsAsPilot", true);
            AlwaysRecieveNotifications = plugin.Config.Bind("DelayedShards", "AlwaysRecieveNotifications", false);
        }
    }
}
