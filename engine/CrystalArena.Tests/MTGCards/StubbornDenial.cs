namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class StubbornDenial
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void CounterAxe()
            {
                Hand(P1, "Lava Axe");
                Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");

                P2.Life = 5;
                Hand(P2, "Stubborn Denial");
                Battlefield(P2, "Island", "Shivan Dragon");

                RunGame(1);

                Equal(1, P1.BreakZone.Count);
                Equal(5, P2.Life);
            }
        }
    }
}
