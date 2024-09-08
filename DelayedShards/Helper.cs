using CG.Game;
using CG.Game.Scenarios;
using Photon.Pun;
using System;
using System.Linq;
using UnityEngine;
using VoidManager.Utilities;

namespace DelayedShards
{
    internal class Helper
    {
        public const long messageTimeout = 8000;

        internal const string EscortID = "Generic_DataShard_OnInsert_SummonEscort";
        internal static AbstractScenarioClassifierAction[] EscortActions;
        internal static int EscortsAvailable = 0;

        internal const string MinefieldID = "Generic_DataShard_OnInsert_DataShard_SummonMinefield";
        internal static AbstractScenarioClassifierAction[] MinefieldActions;
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
            if ((DateTime.Now - lastShardActivated).TotalSeconds < 8) return RejectReason.CooldownTimer;
            if (IsInVoidJump()) return RejectReason.VoidJump;

            Classifier.ClassifierContext context = AbstractScenarioClassifierCondition.BuildContext(null, null, ClientGame.Current.PlayerShip, GameSessionManager.ActiveSector);
            foreach (AbstractScenarioClassifierAction action in EscortActions)
            {
                action.Invoke(context);
            }

            EscortsAvailable--;
            lastShardActivated = Time.time;
            ShardMessageHandler.SendShardCount();
            CheckPreventShardInsert();
            return RejectReason.None;
        }

        internal static RejectReason SummonMinefield()
        {
            if (MinefieldsAvailable <= 0) return RejectReason.MinefieldShardCount;
            if (Time.time - lastShardActivated < 8) return RejectReason.CooldownTimer;
            if (IsInVoidJump()) return RejectReason.VoidJump;

            Classifier.ClassifierContext context = AbstractScenarioClassifierCondition.BuildContext(null, null, ClientGame.Current.PlayerShip, GameSessionManager.ActiveSector);
            foreach (AbstractScenarioClassifierAction action in MinefieldActions)
            {
                action.Invoke(context);
            }
            MinefieldsAvailable--;
            lastShardActivated = DateTime.Now;
            ShardMessageHandler.SendShardCount();
            CheckPreventShardInsert();
            return RejectReason.None;
        }

        internal static void ShardInsertedActivated()
        {
            lastShardActivated = DateTime.Now;
        }

        private static void CheckPreventShardInsert()
        {
            if (Configs.enableQueue.Value) return;

            CarryablesSocketPatch.GalaxyMapSocket.SetState(Gameplay.Carryables.SocketState.Closed);
            Tools.DelayDoUnique(CarryablesSocketPatch.GalaxyMapSocket,
                () => CarryablesSocketPatch.GalaxyMapSocket.SetState(Gameplay.Carryables.SocketState.Open), 8000);
        }

        internal static void Reset()
        {
            EscortActions = null;
            EscortsAvailable = 0;
            MinefieldActions = null;
            MinefieldsAvailable = 0;
            ShardMessageHandler.subscribedPlayers = ShardMessageHandler.subscribedPlayers.Intersect(PhotonNetwork.PlayerListOthers).ToList();
            ShardMessageHandler.SendShardCount();
        }

        internal static bool IsInPilotsSeat(Player player)
        {
            if (player == null) return false;
            TakeoverChair chair = ClientGame.Current?.PlayerShip?.GetModule<Helm>()?.Chair as TakeoverChair;
            return chair != null && !chair.IsAvailable && player == chair.photonView.Owner;
        }

        internal static bool IsInVoidJump()
        {
            VoidJumpState jumpState = ClientGame.Current.PlayerShip.GetComponent<VoidJumpSystem>().ActiveState;
            return jumpState is VoidJumpInterdiction || jumpState is VoidJumpTravellingStable || jumpState is VoidJumpTravellingUnstable || jumpState is VoidJumpApproachingDestination;
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

        public static bool ButtonPressed(UnityEngine.KeyCode key)
        {
            if (key == UnityEngine.KeyCode.None)
                return false;
            return BepInEx.UnityInput.Current.GetKeyDown(key);
        }
    }
}
