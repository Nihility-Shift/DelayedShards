using BepInEx;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using VoidManager.Chat.Router;
using VoidManager.Utilities;

namespace DelayedShards
{
    internal class ActivateShardCommand : PublicCommand
    {
        public override string[] CommandAliases()
        {
            return new string[] { "ActivateShard" };
        }

        public override string Description()
        {
            return "Activates a previously inserted data shard";
        }

        public override List<Argument> Arguments()
        {
            return new List<Argument>() { new Argument( "Escort", "Minefield" ) };
        }

        public override string[] UsageExamples()
        {
            return Arguments()[0].names.Select(name => $"!{CommandAliases()[0]} {name}").ToArray();
        }

        public override void Execute(string arguments, int SenderID)
        {
            if (!Helper.IsInPilotsSeat(Game.GetPlayerFromID(SenderID).PhotonPlayer))
            {
                Messaging.Echo("You must be in the pilot's seat to activate data shards", false);
                return;
            }

            string argument = arguments.Split(' ')[0].ToLower();
            switch (argument)
            {
                case "escort":
                    Helper.SendPublicMessage(Helper.SummonEscort());
                    break;
                case "minefield":
                    Helper.SendPublicMessage(Helper.SummonMinefield());
                    break;
                default:
                    Messaging.Echo("Options are \"Escort\" and \"Minefield\"", false);
                    return;
            }
        }
    }

    internal class CountShardCommand : ActivateShardCommand
    {
        public override string[] CommandAliases()
        {
            return new string[] { "CountShards" };
        }

        public override string Description()
        {
            return "Lists the number of shards available";
        }

        public override void Execute(string arguments, int SenderID)
        {
            if (arguments.IsNullOrWhiteSpace())
            {
                Helper.SendPublicMessage();
                return;
            }

            string argument = arguments.Split(' ')[0].ToLower();
            switch (argument)
            {
                case "escort":
                    Messaging.Echo($"{Helper.EscortsAvailable} Escort shards available", false);
                    break;
                case "minefield":
                    Messaging.Echo($"{Helper.MinefieldsAvailable} Minefield shards available", false);
                    break;
            }
        }
    }
}
