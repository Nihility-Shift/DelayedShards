using CG.Ship.Hull;
using HarmonyLib;

namespace DelayedShards
{
    [HarmonyPatch(typeof(CarryablesSocket), "Start")]
    internal class CarryablesSocketPatch
    {
        static void Prefix(CarryablesSocket __instance)
        {
            if (__instance.name == "GalaxyMapTerminal_SingleSocket")
            {
                __instance.ClosedWhileInVoid = false;
            }
        }
    }
}
