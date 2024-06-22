using BepInEx;
using CG.Input;
using CG;
using Photon.Pun;

namespace DelayedShards
{
    internal class Inputs
    {
        internal static void HandleInputs()
        {
            if (!Helper.IsInPilotsSeat(PhotonNetwork.LocalPlayer))
                return;

            if (!ServiceBase<InputService>.Instance.CursorVisibilityControl.IsCursorShown &&
                Configs.SummonEscortConfig.Value != UnityEngine.KeyCode.None &&
                UnityInput.Current.GetKeyDown(Configs.SummonEscortConfig.Value))
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Helper.RejectReason reason = Helper.SummonEscort();
                    if (reason != Helper.RejectReason.None)
                        Helper.DisplayLocalMessage(reason);
                }
                else
                {
                    ShardMessageHandler.ActivateShard("escort");
                }
            }

            if (!ServiceBase<InputService>.Instance.CursorVisibilityControl.IsCursorShown &&
                Configs.SummonMinefieldConfig.Value != UnityEngine.KeyCode.None &&
                UnityInput.Current.GetKeyDown(Configs.SummonMinefieldConfig.Value))
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Helper.RejectReason reason = Helper.SummonMinefield();
                    if (reason != Helper.RejectReason.None)
                        Helper.DisplayLocalMessage(reason);
                }
                else
                {
                    ShardMessageHandler.ActivateShard("minefield");
                }
            }
        }
    }
}
