using CG.Game.Scenarios.Actions;
using CG.Game.Scenarios;
using Photon.Pun;
using System;
using System.Linq;
using Photon.Realtime;
using CG.Game;
using CG.Ship.Modules;
using CG.Game.SpaceObjects.Controllers;
using VoidManager.Utilities;

namespace DelayedShards
{
    internal class Helper
    {
        public const long messageTimeout = 8000;

        internal static Classifier.ClassifierContext EscortContext;
        internal static AbstractScenarioClassifierAction[] EscortActions;
        internal static int EscortsAvailable = 0;

        internal static Classifier.ClassifierContext MinefieldContext;
        internal static AbstractScenarioClassifierAction[] MinefieldActions;
        internal static int MinefieldsAvailable = 0;

        private static DateTime lastShardActivated = DateTime.MinValue;

        internal enum RejectReason
        {
            None,
            CooldownTimer,
            VoidJump,
            EscortShardCount,
            MinefieldShardCount,
            EscortError,
            MinefieldError
        }

        internal static RejectReason SummonEscort()
        {
            if (EscortsAvailable <= 0) return RejectReason.EscortShardCount;
            if (EscortActions == null) return RejectReason.EscortError;
            if ((DateTime.Now - lastShardActivated).TotalSeconds < 8) return RejectReason.CooldownTimer;
            if (IsInVoidJump()) return RejectReason.VoidJump;

            foreach (AbstractScenarioClassifierAction action in EscortActions)
            {
                action.Invoke(EscortContext);
            }
            EscortsAvailable--;
            lastShardActivated = DateTime.Now;
            ShardMessageHandler.SendShardCount();
            CheckPreventShardInsert();
            return RejectReason.None;
        }

        internal static RejectReason SummonMinefield()
        {
            if (MinefieldsAvailable <= 0) return RejectReason.MinefieldShardCount;
            if (MinefieldActions == null) return RejectReason.MinefieldError;
            if ((DateTime.Now - lastShardActivated).TotalSeconds < 8) return RejectReason.CooldownTimer;
            if (IsInVoidJump()) return RejectReason.VoidJump;

            foreach (AbstractScenarioClassifierAction action in MinefieldActions)
            {
                action.Invoke(MinefieldContext);
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
            EscortContext = null;
            EscortActions = null;
            EscortsAvailable = 0;
            MinefieldContext = null;
            MinefieldContext = null;
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
                RejectReason.EscortError => "Error caused by host leaving, insert escort shard to fix",
                RejectReason.MinefieldError => "Error caused by host leaving, insert minefield shard to fix",
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
