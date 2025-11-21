namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class RadiantArchangel
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void RadiantIs66()
            {
                var radiant = C("Radiant, Archangel");

                Hand(P1, "Spire Owl");
                Battlefield(P1, radiant, "Island", "Plains", "Spire Owl");
                Battlefield(P2, "Wall of Denial");

                RunGame(1);

                Equal(6, C(radiant).Power);
            }
        }
    }
}
