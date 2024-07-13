using CG.Client;
using CG.Game;
using Photon.Pun;
using UI.MainHUD;
using UnityEngine;

namespace DelayedShards
{
    internal class DataShardGUI : MonoBehaviour
    {
        private bool mainUiEnabled = true;
        private bool guiActive = false;
        internal Rect WindowPos;
        internal const float width = 100;
        internal const float height = 74;

        internal static DataShardGUI Instance { get; private set; }

        private DataShardGUI()
        {
            Instance = this;
            VoidManager.Utilities.Tools.DelayDo(() => ViewEventBus.Instance.OnUIToggle.Subscribe(enable => mainUiEnabled = enable), 12000);
        }

        internal void UpdateWindowPos()
        {
            WindowPos = new Rect(Screen.width * Configs.GUIPosX.Value, Screen.height * Configs.GUIPosY.Value, width, height);
        }

        private void Update()
        {
            bool shouldBeActive = Configs.hostHasMod && mainUiEnabled && (Configs.AlwaysDisplayGUI.Value || (Configs.DisplayGUIAsPilot.Value && Helper.IsInPilotsSeat(PhotonNetwork.LocalPlayer))) && ClientGame.Current?.PlayerShip?.InteriorAtmosphere != null;
            if (shouldBeActive != guiActive)
            {
                guiActive = !guiActive;
                WindowPos = new(Screen.width * Configs.GUIPosX.Value, Screen.height * Configs.GUIPosY.Value, width, height);
            }
        }

        private void OnGUI()
        {
            if (guiActive)
            {
                UnityEngine.GUI.Window(818107, WindowPos, WindowFunction, "Data Shards");
            }
        }

        private void WindowFunction(int windowID)
        {
            GUILayout.Label($"Escort:    {Helper.EscortsAvailable}");
            GUILayout.Label($"Minefield: {Helper.MinefieldsAvailable}");
        }
    }
}
