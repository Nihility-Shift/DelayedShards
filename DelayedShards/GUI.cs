using UnityEngine;
using VoidManager.CustomGUI;
using VoidManager.Utilities;
using static UnityEngine.GUILayout;

namespace DelayedShards
{
    internal class GUI : ModSettingsMenu
    {
        public override string Name() => "Delayed Shards";

        public override void Draw()
        {
            Label($"Escort shards available: {Helper.EscortsAvailable}");
            Label($"Minefield shards available: {Helper.MinefieldsAvailable}");

            bool recieveNotificationsAsPilot = Configs.RecieveNotificationsAsPilot.Value;
            if (GUITools.DrawCheckbox("Display notifications when in the pilots seat and shard count changes", ref recieveNotificationsAsPilot))
            {
                Configs.RecieveNotificationsAsPilot.Value = recieveNotificationsAsPilot;
            }
            bool alwaysRecieveNotifications = Configs.AlwaysRecieveNotifications.Value;
            if (GUITools.DrawCheckbox("Always display notifications when shard count changes", ref alwaysRecieveNotifications))
            {
                Configs.AlwaysRecieveNotifications.Value = alwaysRecieveNotifications;
            }

            KeyCode escortKeyCode = Configs.SummonEscortConfig.Value;
            if (GUITools.DrawChangeKeybindButton("Change Summon Escort Keybind", ref escortKeyCode)) {
                Configs.SummonEscortConfig.Value = escortKeyCode;
            }
            KeyCode minefieldKeyCode = Configs.SummonMinefieldConfig.Value;
            if (GUITools.DrawChangeKeybindButton("Change Summon Minefield Keybind", ref minefieldKeyCode))
            {
                Configs.SummonMinefieldConfig.Value = minefieldKeyCode;
            }
        }
    }
}
