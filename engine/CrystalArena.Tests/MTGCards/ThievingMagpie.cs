namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class ThievingMagpie
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void DrawCard()
            {
                Battlefield(P1, "Thieving Magpie");

                RunGame(1);
                Equal(1, P1.Hand.Count);
            }
        }
    }
}
