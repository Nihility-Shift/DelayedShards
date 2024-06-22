using CG.Game.Scenarios;
using CG.Game.Scenarios.Actions;
using CG.Game.Scenarios.Conditions;
using HarmonyLib;
using System.Reflection;
using static DelayedShards.Helper;

namespace DelayedShards
{
    [HarmonyPatch(typeof(Classifier), "OnTriggerConditionChanged")]
    internal class ClassifierPatch
    {
        private static readonly FieldInfo delayMsField = AccessTools.Field(typeof(Timer), "delayMs");

        static bool Prefix(Classifier __instance, AbstractScenarioClassifierCondition abstractScenarioClassifierCondition, bool value, Classifier.ClassifierContext context)
        {
            if (!value) return true;

            if (__instance.Id == "Generic_DataShard_OnInsert_SummonEscort")
            {
                if (EscortActions == null)
                {
                    EscortContext = context;
                    EscortActions = __instance.Actions;
                    delayMsField.SetValue((Timer)EscortActions[0], 0);
                    delayMsField.SetValue((Timer)EscortActions[2], 2000);
                }
                __instance.HasBeenInvoked = true;
                EscortsAvailable++;
                ShardMessageHandler.SendShardCount();
                return false;
            }
            else if (__instance.Id == "Generic_DataShard_OnInsert_DataShard_SummonMinefield")
            {
                if (MinefieldActions == null)
                {
                    MinefieldContext = context;
                    MinefieldActions = __instance.Actions;
                    delayMsField.SetValue((Timer)MinefieldActions[0], 0);
                    delayMsField.SetValue((Timer)MinefieldActions[1], 8000);
                }
                __instance.HasBeenInvoked = true;
                MinefieldsAvailable++;
                ShardMessageHandler.SendShardCount();
                return false;
            }

            return true;
        }
    }
}
