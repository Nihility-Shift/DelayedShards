using CG.Ship.Hull;
using HarmonyLib;

namespace DelayedShards.Functionality
{
    [HarmonyPatch(typeof(CarryablesSocket), "Start")]
    internal class CarryablesSocketPatch
    {
        internal static CarryablesSocket GalaxyMapSocket;
        internal static bool InVoid { get; private set; } = false;

        static void Prefix(CarryablesSocket __instance)
        {
            if (__instance.name == "Socket_AstralMap")
            {
                GalaxyMapSocket = __instance;
                __instance.ClosedWhileInVoid = false;
                GameSessionSectorManager.OnSectorExited += PlayerShipEnteredVoid;
                GameSessionSectorManager.OnSectorEntered += OnPlayerShipSectorEnter;
            }
        }

        internal static void ToggleQueue()
        {
            if (InVoid)
            {
                if (Configs.enableQueue.Value)
                {
                    GalaxyMapSocket.closedBecauseInVoid = false;
                    GalaxyMapSocket.SetState(Gameplay.Carryables.SocketState.Open);
                }
                else
                {
                    GalaxyMapSocket.SetState(Gameplay.Carryables.SocketState.Closed);
                    GalaxyMapSocket.closedBecauseInVoid = true;
                }
            }
        }

        private static void OnPlayerShipSectorEnter(GameSessionSector sector)
        {
            InVoid = false;
            if (!Configs.enableQueue.Value)
            {
                GalaxyMapSocket.closedBecauseInVoid = false;
                GalaxyMapSocket.SetState(Gameplay.Carryables.SocketState.Open);
            }
        }

        private static void PlayerShipEnteredVoid(GameSessionSector sector)
        {
            InVoid = true;
            if (!Configs.enableQueue.Value)
            {
                GalaxyMapSocket.SetState(Gameplay.Carryables.SocketState.Closed);
                GalaxyMapSocket.closedBecauseInVoid = true;
            }
        }
    }
}
