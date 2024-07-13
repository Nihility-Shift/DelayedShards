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
            Label("");
            KeyCode escortKeyCode = Configs.SummonEscortConfig.Value;
            if (GUITools.DrawChangeKeybindButton("Change Summon Escort Keybind", ref escortKeyCode))
            {
                Configs.SummonEscortConfig.Value = escortKeyCode;
            }
            KeyCode minefieldKeyCode = Configs.SummonMinefieldConfig.Value;
            if (GUITools.DrawChangeKeybindButton("Change Summon Minefield Keybind", ref minefieldKeyCode))
            {
                Configs.SummonMinefieldConfig.Value = minefieldKeyCode;
            }

            Label("");
            GUITools.DrawCheckbox("Display GUI when in the pilots seat", ref Configs.DisplayGUIAsPilot);
            GUITools.DrawCheckbox("Always display GUI", ref Configs.AlwaysDisplayGUI);

            Label("");
            BeginHorizontal();
            FlexibleSpace();
            Label("GUI Position");
            FlexibleSpace();
            EndHorizontal();
            Label("x: ");
            if (GUITools.DrawSlider(ref Configs.GUIPosX, 0, 1))
            {
                DataShardGUI.Instance.UpdateWindowPos();
            }
            Label("y: ");
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
