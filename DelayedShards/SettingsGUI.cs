using DelayedShards.Functionality;
using VoidManager.CustomGUI;
using VoidManager.Utilities;
using static UnityEngine.GUILayout;

namespace DelayedShards
{
    internal class SettingsGUI : ModSettingsMenu
    {
        public override string Name() => MyPluginInfo.USERS_PLUGIN_NAME;

        public override void Draw()
        {
            Label("");
            if (GUITools.DrawCheckbox("Add inserted shards to queue (host)", ref Configs.enableQueue))
            {
                CarryablesSocketPatch.ToggleQueue();
            }
            GUITools.DrawCheckbox("Display GUI when in the pilots seat (client)", ref Configs.DisplayGUIAsPilot);
            GUITools.DrawCheckbox("Always display GUI (client)", ref Configs.AlwaysDisplayGUI);
            GUITools.DrawCheckbox("Use Void Manager GUI style", ref Configs.VoidManagerUIStyle);

            Label("");
            GUITools.DrawChangeKeybindButton("Change Summon Escort Keybind", ref Configs.SummonEscortConfig);
            GUITools.DrawChangeKeybindButton("Change Summon Minefield Keybind", ref Configs.SummonMinefieldConfig);

            Label("");
            BeginHorizontal();
            FlexibleSpace();
            Label("GUI Position");
            FlexibleSpace();
            EndHorizontal();
            Label($"X: {Configs.GUIPosX.Value.ToString("P")}");
            if (GUITools.DrawSlider(ref Configs.GUIPosX, 0, 1))
            {
                DataShardGUI.Instance.UpdateWindowPos();
            }
            Label($"Y: {Configs.GUIPosY.Value.ToString("P")}");
            if (GUITools.DrawSlider(ref Configs.GUIPosY, 0, 1))
            {
                DataShardGUI.Instance.UpdateWindowPos();
            }
            if (Button("Reset"))
            {
                Configs.GUIPosX.Value = (float)Configs.GUIPosX.DefaultValue;
                Configs.GUIPosY.Value = (float)Configs.GUIPosY.DefaultValue;
                DataShardGUI.Instance.UpdateWindowPos();
            }
        }
    }
}
