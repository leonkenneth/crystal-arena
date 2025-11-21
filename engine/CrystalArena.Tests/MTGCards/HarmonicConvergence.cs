namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class HarmonicConvergence
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void ReturnArmorToTopOfMainDeckAndBlock()
            {
                var bear1 = C("Grizzly Bears");
                var bear2 = C("Grizzly Bears");
                var anthem = C("Glorious Anthem");
                var exploration = C("Exploration");

                Battlefield(
                    P1,
                    bear1.IsEnchantedWith("Blanchwood Armor"),
                    bear2.IsEnchantedWith("Blanchwood Armor"),
                    "Forest",
                    "Forest",
                    "Forest",
                    "Forest"
                );
                Battlefield(
                    P2,
                    "Trained Armodon",
                    "Trained Armodon",
                    "Forest",
                    "Plains",
                    "Plains",
                    anthem,
                    exploration
                );
                Hand(P2, "Harmonic Convergence");

                RunGame(1);

                Equal(Zone.BreakZone, C(bear1).Zone);
                Equal(Zone.BreakZone, C(bear2).Zone);
                Equal(C(anthem), P2.MainDeck.First());
            }
        }
    }
}
