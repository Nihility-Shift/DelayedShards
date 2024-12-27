using CG.Ship.Hull;
using HarmonyLib;

namespace DelayedShards.Functionality
{
    //By default map socket is closed in void. Overrides vanilla closing with own closing method, which is enabled/disabled when configured.
    [HarmonyPatch(typeof(CarryablesSocket))]
    internal class CarryablesSocketPatch
    {
        internal static CarryablesSocket GalaxyMapSocket;

        [HarmonyPatch("Start"), HarmonyPrefix]
        static void StartPatch(CarryablesSocket __instance)
        {
            if (__instance.name == "Socket_AstralMap")
            {
                GalaxyMapSocket = __instance;
                __instance.ClosedWhileInVoid = false;
                GameSessionSectorManager.OnSectorExited += PlayerShipEnteredVoid;
                GameSessionSectorManager.OnSectorEntered += OnPlayerShipSectorEnter;
                BepinPlugin.Log.LogInfo("patched astral map socket");
            }
        }

        [HarmonyPatch("OnDestroy"), HarmonyPostfix]
        static void DestroyPatch()
        {
            GameSessionSectorManager.OnSectorExited -= PlayerShipEnteredVoid;
            GameSessionSectorManager.OnSectorEntered -= OnPlayerShipSectorEnter;
        }

        //Allows internal toggling of socket close state when mod is enabled/disabled
        internal static void ToggleQueue()
        {
            if (Helper.IsInVoidJump())
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
            if (!Configs.enableQueue.Value)
            {
                GalaxyMapSocket.closedBecauseInVoid = false;
                GalaxyMapSocket.SetState(Gameplay.Carryables.SocketState.Open);
            }
        }

        private static void PlayerShipEnteredVoid(GameSessionSector sector)
        {
            if (!Configs.enableQueue.Value)
            {
                GalaxyMapSocket.SetState(Gameplay.Carryables.SocketState.Closed);
                GalaxyMapSocket.closedBecauseInVoid = true;
            }
        }
    }
}
