using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrorTown;

namespace Sandbox
{
    public partial class Seance : BaseTeam
    {
        public override TeamAlignment TeamAlignment => TeamAlignment.Innocent;
        public override string TeamName => "Seance";
        public override Color TeamColour => Color.Green;
        public override TeamMemberVisibility TeamMemberVisibility => TeamMemberVisibility.None;
        public override string VictimKillMessage => "You were killed by {0}. They were an innocent seance.";

        public override string RoleDescription => @"You are an innocent Seance! You can see indicators of dead people.

Work with the innocent Terrorists to find the Traitors.";

        public override string IdentifyString => "{0} found the body of {1}. They were an innocent seance.";

        [ConVar.Server(
            "cutie_players_per_seance",
            Help = "Assign one seance per specified player count. For example, setting this to 10 will mean that there will be one seance per 7 players. The default is 7.",
            Saved = true
        )]
        public static int PlayersPerSeance { get; set; } = 7;

        public override float TeamPlayerPercentage => 1f / PlayersPerSeance;

        private static Dictionary<TerrorTown.Player, WorldIndicatorPanel> Seances = new();

        [Event("Player.PostOnKilled")]
        public static void AddOrbToPlayer(DamageInfo _, TerrorTown.Player ply) => AddOrbToPlayerRpc(ply);

        [Event("Player.PostSpawn")]
        public static void RemoveOrbFromPlayer(TerrorTown.Player ply) => RemoveOrbFromPlayerRpc(ply);

        [ClientRpc]
        public static void AddOrbToPlayerRpc(TerrorTown.Player ply)
        {
            Seances[ply] = new WorldIndicatorPanel(new List<string>() { "Seance" }, dangerousTarget: ply, color: Color.Yellow, onlyWhenAlive: true);
        }

        [ClientRpc]
        public static void RemoveOrbFromPlayerRpc(TerrorTown.Player ply)
        {
            if( Seances.TryGetValue(ply, out WorldIndicatorPanel panel))
            {
                panel.Delete();
            }
        }
    }
}
