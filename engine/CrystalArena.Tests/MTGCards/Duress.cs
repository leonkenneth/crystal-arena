namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Duress
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void DiscardOneCard()
            {
                var counterspell = C("Counterspell");

                Hand(P1, "Duress");
                Battlefield(P1, "Swamp");

                Hand(P2, counterspell, "Island");

                RunGame(1);

                Equal(Zone.BreakZone, C(counterspell).Zone);
            }
        }
    }
}
