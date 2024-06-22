using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using VoidManager.ModMessages;

namespace DelayedShards
{
    internal class ShardMessageHandler : ModMessage
    {
        internal static List<Player> subscribedPlayers = new();

        private enum Requests
        {
            Subscribe,
            Sent,
            Activate,
            Reject
        }

        public override void Handle(object[] arguments, Player sender)
        {
            if (arguments.Length == 0) return;
            if ((string)arguments[0] != MyPluginInfo.PLUGIN_VERSION)
            {
                BepinPlugin.Log.LogInfo($"Got version {(string)arguments[0]} from {sender.NickName}, expected version {MyPluginInfo.PLUGIN_VERSION}");
                return;
            }

            switch ((Requests)arguments[1])
            {
                case Requests.Subscribe:
                    if (!PhotonNetwork.IsMasterClient) break;
                    if (!subscribedPlayers.Contains(sender))
                        subscribedPlayers.Add(sender);
                    SendShardCount(sender);
                    break;

                case Requests.Sent:
                    if (PhotonNetwork.IsMasterClient || sender != PhotonNetwork.MasterClient) break;
                    int[] shards = (int[])arguments[2];
                    Helper.EscortsAvailable = shards[0];
                    Helper.MinefieldsAvailable = shards[1];
                    Helper.DisplayLocalMessage();
                    break;

                case Requests.Activate:
                    if (!PhotonNetwork.IsMasterClient) break;
                    Helper.RejectReason reason = Helper.RejectReason.None;
                    switch(((string)arguments[2]).ToLower())
                    {
                        case "escort":
                            reason = Helper.SummonEscort();
                            break;
                        case "minefield":
                            reason = Helper.SummonMinefield();
                            break;
                    }
                    if (reason != Helper.RejectReason.None)
                    {
                        SendRejectMessage(sender, reason);
                    }
                    break;

                case Requests.Reject:
                    if (PhotonNetwork.IsMasterClient || sender != PhotonNetwork.MasterClient) break;
                    Helper.DisplayLocalMessage((Helper.RejectReason)arguments[2]);
                    break;
            }
        }

        internal static void SubscribeToCountUpdates()
        {
            if (PhotonNetwork.IsMasterClient) return;

            Send(MyPluginInfo.PLUGIN_GUID, GetIdentifier(typeof(ShardMessageHandler)), PhotonNetwork.MasterClient,
                new object[] { MyPluginInfo.PLUGIN_VERSION, Requests.Subscribe });
        }

        internal static void ActivateShard(string shardName)
        {
            if (PhotonNetwork.IsMasterClient) return;

            Send(MyPluginInfo.PLUGIN_GUID, GetIdentifier(typeof(ShardMessageHandler)), PhotonNetwork.MasterClient,
                new object[] { MyPluginInfo.PLUGIN_VERSION, Requests.Activate, shardName });
        }

        internal static void SendShardCount(params Player[] player)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (player == null || player.Length == 0)
            {
                player = subscribedPlayers.ToArray();
                Helper.DisplayLocalMessage();
            }

            Send(MyPluginInfo.PLUGIN_GUID, GetIdentifier(typeof(ShardMessageHandler)), player,
                new object[] { MyPluginInfo.PLUGIN_VERSION, Requests.Sent, new int[] { Helper.EscortsAvailable, Helper.MinefieldsAvailable } });
        }

        internal static void SendRejectMessage(Player player, Helper.RejectReason reason)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            Send(MyPluginInfo.PLUGIN_GUID, GetIdentifier(typeof(ShardMessageHandler)), player,
                new object[] { MyPluginInfo.PLUGIN_VERSION, Requests.Reject, reason });
        }
    }
}
