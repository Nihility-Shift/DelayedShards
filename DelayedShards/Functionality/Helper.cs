using Photon.Pun;
using Photon.Realtime;
using ResourceAssets;
using System;
using System.Linq;
using UnityEngine;
using VoidManager.Utilities;

namespace DelayedShards.Functionality
{
    internal class Helper
    {
        public const long messageTimeout = 8000;

        internal static GUIDUnion EscortGUID = new GUIDUnion("db0a40ea1d616fa4eb80b044dc3efa3e");
        internal static int EscortsAvailable = 0;

        internal static GUIDUnion MinefieldGUID = new GUIDUnion("d2427729f742ec04d98aa89b0575e9e0");
        internal static int MinefieldsAvailable = 0;

        private static float lastShardActivated = 0f;

        internal enum RejectReason
        {
            None,
            CooldownTimer,
            VoidJump,
            EscortShardCount,
            MinefieldShardCount,
        }

        internal static RejectReason SummonEscort()
        {
            if (EscortsAvailable <= 0) return RejectReason.EscortShardCount;
            if (Time.time - lastShardActivated < 8) return RejectReason.CooldownTimer;
            if (IsInVoidJump()) return RejectReason.VoidJump;

            CarryableContainer.Instance.GetAssetDefById(EscortGUID).Asset.GetComponent<CarryableSummonConsumptionEffect>().SummonUnits();

            EscortsAvailable--;
            lastShardActivated = Time.time;
            ShardMessageHandler.SendShardCount();
            return RejectReason.None;
        }

        //Attempt summon minefield.
        internal static RejectReason SummonMinefield()
        {
            if (MinefieldsAvailable <= 0) return RejectReason.MinefieldShardCount;
            if (Time.time - lastShardActivated < 8) return RejectReason.CooldownTimer;
            if (IsInVoidJump()) return RejectReason.VoidJump;

            CarryableContainer.Instance.GetAssetDefById(MinefieldGUID).Asset.GetComponent<CarryableSummonConsumptionEffect>().SummonUnits();

            MinefieldsAvailable--;
            lastShardActivated = Time.time;
            ShardMessageHandler.SendShardCount();
            return RejectReason.None;
        }

        internal static void Reset()
        {
            EscortsAvailable = 0;
            MinefieldsAvailable = 0;
            ShardMessageHandler.subscribedPlayers = ShardMessageHandler.subscribedPlayers.Intersect(PhotonNetwork.PlayerListOthers).ToList();
            ShardMessageHandler.SendShardCount();
        }

        internal static bool IsInPilotsSeat(Player player)
        {
            if (player == null) return false;

            return Game.CurrentPilot?.PhotonPlayer == player;
        }

        internal static bool IsInVoidJump()
        {
            return GameSessionSectorManager.Instance.Status != GameSessionSectorManager.SectorTransitionStatus.InSector;
        }

        internal static void SendPublicMessage(RejectReason reason = RejectReason.None)
        {
            Messaging.Echo(GetDisplayMessage(reason), false);
        }

        internal static void DisplayLocalMessage(RejectReason reason = RejectReason.None)
        {
            if (reason != RejectReason.None)
                Messaging.Notification(GetDisplayMessage(reason), messageTimeout);
        }

        private static string GetDisplayMessage(RejectReason reason)
        {
            return reason switch
            {
                RejectReason.None => $"{EscortsAvailable} escort shards available\n{MinefieldsAvailable} minefield shards available",
                RejectReason.CooldownTimer => "Data shards on cooldown",
                RejectReason.VoidJump => "Data shards not available during void jump",
                RejectReason.EscortShardCount => "No escort shards available",
                RejectReason.MinefieldShardCount => "No minefield shards available",
                _ => throw new ArgumentException(),
            };
        }

        public static bool ButtonPressed(KeyCode key)
        {
            if (key == KeyCode.None)
                return false;
            return BepInEx.UnityInput.Current.GetKeyDown(key);
        }
    }
}
