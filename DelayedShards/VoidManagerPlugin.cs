using VoidManager.MPModChecks;

namespace DelayedShards
{
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
