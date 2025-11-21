namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Exhume
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void ExhumeForce()
            {
                Hand(P1, "Exhume");

                Battlefield(P1, "Swamp", "Swamp");
                BreakZone(P1, "Verdant Force");
                BreakZone(P2, "Trained Armodon");

                RunGame(1);

                Equal(3, P1.Battlefield.Count());
                Equal(1, P2.Battlefield.Count());
            }
        }
    }
}
