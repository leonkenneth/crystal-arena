namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class ReturnToTheRanks
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void ReturnBearsFromBreakZone()
            {
                Hand(P1, "Return To The Ranks");
                BreakZone(P1, "Grizzly Bears", "Grizzly Bears");
                Battlefield(P1, "Plains", "Plains", "Plains", "Wall of Frost");

                RunGame(1);

                Equal(3, P1.Battlefield.Forwards.Count());
            }
        }
    }
}
