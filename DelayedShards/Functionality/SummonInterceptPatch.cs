using HarmonyLib;

namespace DelayedShards.Functionality
{
    //Block origional consumption code from running and incriment counter.
    [HarmonyPatch(typeof(CarryableSummonConsumptionEffect), "OnConsume")]
    internal class SummonInterceptPatch
    {
        static bool Prefix(CarryableSummonConsumptionEffect __instance)
        {
            if (!Configs.enableQueue.Value) return true;

            if (__instance.carryable.assetGuid == Helper.MinefieldGUID)
            {
                Helper.MinefieldsAvailable++;
                ShardMessageHandler.SendShardCount();
                return false;
            }
            else if (__instance.carryable.assetGuid == Helper.EscortGUID)
            {
                Helper.EscortsAvailable++;
                ShardMessageHandler.SendShardCount();
                return false;
            }
            return true;
        }
    }
}
