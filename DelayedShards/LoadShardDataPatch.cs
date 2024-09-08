using CG.Game.Scenarios.Actions;
using HarmonyLib;
using System.Linq;
using System.Reflection;
using static DelayedShards.Helper;

namespace DelayedShards
{
    //Load classifiers from existing quest data and classifier values to static var.
    [HarmonyPatch(typeof(AnalyticsManager), "SendGameSessionStart")]
    internal class LoadShardDataPatch
    {
        private static readonly FieldInfo delayMsField = AccessTools.Field(typeof(Timer), "delayMs");
        [HarmonyPostfix]
        static void LoadClassifiersPatch(string missionName)
        {
            if (missionName == "HUB") return;
            BepinPlugin.Log.LogInfo("Caching data after quest load");
            EscortActions = GameSessionManager.ActiveSession.ActiveQuest.Classifiers.First(classi => classi.Id == EscortID).Actions;
            delayMsField.SetValue((Timer)EscortActions[0], 0);
            delayMsField.SetValue((Timer)EscortActions[2], 2000);

            MinefieldActions = GameSessionManager.ActiveSession.ActiveQuest.Classifiers.First(classi => classi.Id == MinefieldID).Actions;
            delayMsField.SetValue((Timer)MinefieldActions[0], 0);
            delayMsField.SetValue((Timer)MinefieldActions[1], 8000);
        }
    }
}
