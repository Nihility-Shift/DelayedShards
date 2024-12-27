using CG.Objects;
using CG.Ship.Hull;
using HarmonyLib;
using Photon.Pun;

namespace DelayedShards.Functionality
{
    //Block origional consumption code from running, dispose of payload, and incriment counter.
    [HarmonyPatch(typeof(SocketPayloadConsumer), "OnAcquireCarryable")]
    internal class SummonInterceptPatch
    {
        static bool Prefix(SocketPayloadConsumer __instance, CarryableObject carryable)
        {
            if (!Configs.enableQueue.Value || !PhotonNetwork.IsMasterClient) return true;

            if (carryable.assetGuid == Helper.MinefieldGUID)
            {
                Helper.MinefieldsAvailable++;
                ShardMessageHandler.SendShardCount();
                __instance.ConsumePayload();
                return false;
            }
            else if (carryable.assetGuid == Helper.EscortGUID)
            {
                Helper.EscortsAvailable++;
                ShardMessageHandler.SendShardCount();
                __instance.ConsumePayload();
                return false;
            }
            return true;
        }
    }
}
