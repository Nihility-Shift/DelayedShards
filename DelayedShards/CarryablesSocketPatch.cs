using CG.Game.SpaceObjects.Controllers;
using CG.Game;
using CG.Ship.Hull;
using HarmonyLib;
using System.Reflection;

namespace DelayedShards
{
    [HarmonyPatch(typeof(CarryablesSocket), "Start")]
    internal class CarryablesSocketPatch
    {
        private static readonly FieldInfo closedBecauseInVoidField = AccessTools.Field(typeof(CarryablesSocket), "closedBecauseInVoid");

        internal static CarryablesSocket GalaxyMapSocket;
        internal static bool InVoid { get; private set; } = false;

        static void Prefix(CarryablesSocket __instance)
        {
            if (__instance.name == "GalaxyMapTerminal_SingleSocket")
            {
                GalaxyMapSocket = __instance;
                __instance.ClosedWhileInVoid = false;
                VoidJumpSystem voidJumpSystem = ClientGame.Current.PlayerShip.GetComponent<VoidJumpSystem>();
                voidJumpSystem.OnVoidEntered += PlayerShipEnteredVoid;
                voidJumpSystem.OnSectorEnter += OnPlayerShipSectorEnter;
            }
        }

        internal static void ToggleQueue()
        {
            if (InVoid)
            {
                if (Configs.enableQueue.Value)
                {
                    closedBecauseInVoidField.SetValue(GalaxyMapSocket, false);
                    GalaxyMapSocket.SetState(Gameplay.Carryables.SocketState.Open);
                }
                else
                {
                    GalaxyMapSocket.SetState(Gameplay.Carryables.SocketState.Closed);
                    closedBecauseInVoidField.SetValue(GalaxyMapSocket, true);
                }
            }
        }

        private static void OnPlayerShipSectorEnter(GameSessionSector sector)
        {
            InVoid = false;
            if (!Configs.enableQueue.Value)
            {
                closedBecauseInVoidField.SetValue(GalaxyMapSocket, false);
                GalaxyMapSocket.SetState(Gameplay.Carryables.SocketState.Open);
            }
        }

        private static void PlayerShipEnteredVoid()
        {
            InVoid = true;
            if (!Configs.enableQueue.Value)
            {
                GalaxyMapSocket.SetState(Gameplay.Carryables.SocketState.Closed);
                closedBecauseInVoidField.SetValue(GalaxyMapSocket, true);
            }
        }
    }
}
