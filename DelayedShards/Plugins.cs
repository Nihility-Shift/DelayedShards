using BepInEx;
using BepInEx.Logging;
using DelayedShards.Controls;
using DelayedShards.Functionality;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using VoidManager.MPModChecks;

namespace DelayedShards
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.USERS_PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Void Crew.exe")]
    [BepInDependency(VoidManager.MyPluginInfo.PLUGIN_GUID)]
    public class BepinPlugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private void Awake()
        {
            Log = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
            Configs.Load(this);
            new GameObject("DataShardGUI", typeof(DataShardGUI)) { hideFlags = HideFlags.HideAndDontSave };
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
    }

    public class VoidManagerPlugin : VoidManager.VoidPlugin
    {
        public VoidManagerPlugin()
        {
            VoidManager.Events.Instance.JoinedRoom += (sender, e) => ShardMessageHandler.SubscribeToCountUpdates();
            VoidManager.Events.Instance.LeftRoom += (sender, e) => Helper.Reset();
            VoidManager.Events.Instance.HostStartSession += (sender, e) => Helper.Reset();
            VoidManager.Events.Instance.MasterClientSwitched += (sender, e) => ShardMessageHandler.SubscribeToCountUpdates();
            VoidManager.Events.Instance.PlayerLeftRoom += (sender, e) => ShardMessageHandler.subscribedPlayers.Remove(e.player);
            VoidManager.Events.Instance.LateUpdate += (sender, e) => Inputs.HandleInputs();
        }

        public override MultiplayerType MPType => MultiplayerType.Session;

        public override string Author => MyPluginInfo.PLUGIN_AUTHORS;

        public override string Description => MyPluginInfo.PLUGIN_DESCRIPTION;

        public override string ThunderstoreID => MyPluginInfo.PLUGIN_THUNDERSTORE_ID;
    }
}