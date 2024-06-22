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

        public override MultiplayerType MPType => MultiplayerType.Host;

        public override string Author => "18107";

        public override string Description => "Inserting shards into the map adds them to a queue for the pilot to activate at any time";
    }
}
