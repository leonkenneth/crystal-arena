namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Scrapheap
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Gain1LifeForRing()
            {
                Battlefield(P1, "Scrapheap", "Ring of Gix");

                RunGame(1);
                Equal(21, P1.Life);
            }
        }
    }
}
