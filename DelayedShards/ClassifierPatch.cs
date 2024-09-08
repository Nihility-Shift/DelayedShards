using CG.Game.Scenarios;
using CG.Game.Scenarios.Conditions;
using HarmonyLib;
using static DelayedShards.Helper;

namespace DelayedShards
{
    //Stop shards insertions early and incriment related counter. 
    [HarmonyPatch(typeof(Classifier), "OnTriggerConditionChanged")]
    internal class ClassifierPatch
    {
        static bool Prefix(Classifier __instance, AbstractScenarioClassifierCondition abstractScenarioClassifierCondition, bool value, Classifier.ClassifierContext context)
        {
            if (!Configs.enableQueue.Value)
            {
                if (__instance.Id == "Generic_DataShard_OnInsert_SummonEscort" || __instance.Id == "Generic_DataShard_OnInsert_DataShard_SummonMinefield")
                {
                    ShardInsertedActivated(); //set delay in case of fast queue mode switch.
                }
                return true;
            }

            if (!value) return true;

            if (__instance.Id == "Generic_DataShard_OnInsert_SummonEscort")
            {
                __instance.HasBeenInvoked = true;
                EscortsAvailable++;
                ShardMessageHandler.SendShardCount();
                return false;
            }
            else if (__instance.Id == "Generic_DataShard_OnInsert_DataShard_SummonMinefield")
            {
                __instance.HasBeenInvoked = true;
                MinefieldsAvailable++;
                ShardMessageHandler.SendShardCount();
                return false;
            }

            return true;
        }
    }
}
