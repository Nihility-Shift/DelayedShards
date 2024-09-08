using CG.Client;
using CG.Game;
using Photon.Pun;
using UnityEngine;
using static UnityEngine.GUILayout;

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
                if (Configs.VoidManagerUIStyle.Value)
                    GUI.skin = ChangeSkin();
                GUI.Window(818107, WindowPos, WindowFunction, "Data Shards");
            }
        }

        private void WindowFunction(int windowID)
        {
            Label($"Escort:    {Helper.EscortsAvailable}");
            Label($"Minefield: {Helper.MinefieldsAvailable}");
        }

        
        static GUISkin _cachedSkin;
        static readonly Color32 _classicMenuBackground = new Color32(32, 32, 32, 255);
        private GUISkin ChangeSkin()
        {
            if (_cachedSkin is null || _cachedSkin.window.active.background is null)
            {
                _cachedSkin = Instantiate(GUI.skin);
                Texture2D windowBackground = BuildTexFrom1Color(_classicMenuBackground);
                _cachedSkin.window.active.background = windowBackground;
                _cachedSkin.window.onActive.background = windowBackground;
                _cachedSkin.window.focused.background = windowBackground;
                _cachedSkin.window.onFocused.background = windowBackground;
                _cachedSkin.window.hover.background = windowBackground;
                _cachedSkin.window.onHover.background = windowBackground;
                _cachedSkin.window.normal.background = windowBackground;
                _cachedSkin.window.onNormal.background = windowBackground;
            }
            return _cachedSkin;
        }
        

        Texture2D BuildTexFrom1Color(Color color)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, color);
            tex.Apply();
            return tex;
        }
    }
}
