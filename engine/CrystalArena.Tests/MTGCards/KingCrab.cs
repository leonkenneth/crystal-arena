namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class KingCrab
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PutForceOnTopOfMainDeck()
            {
                var force = C("Verdant Force");

                Battlefield(P1, "King Crab", "Island", "Island");
                Battlefield(P2, force);

                RunGame(1);

                Equal(Zone.MainDeck, C(force).Zone);
            }
        }
    }
}
