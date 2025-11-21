namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class PeaceAndQuiet
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void DestroyEmbraceWorship()
            {
                Hand(P1, "Peace And Quiet");
                Battlefield(P1, "Plains", "Plains");
                Battlefield(P2, C("Grizzly Bears").IsEnchantedWith("Gaea's Embrace"), "Worship");

                RunGame(2);

                Equal(2, P2.BreakZone.Count(c => c.Is().Monster));
            }
        }
    }
}
