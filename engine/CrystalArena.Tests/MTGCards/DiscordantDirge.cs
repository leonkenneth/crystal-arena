namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class DiscordantDirge
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Discard3()
            {
                Battlefield(P1, "Discordant Dirge", "Swamp");
                Hand(P2, "Ravenous Baloth", "Verdant Force", "Llanowar Elves", "Grizzly Bears");

                RunGame(6);

                Equal(3, P2.BreakZone.Count());
            }
        }
    }
}
