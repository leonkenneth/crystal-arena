namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class HeliodsPilgrim
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PutFavorIntoHand()
            {
                var favor = C("Divine Favor");
                MainDeck(P1, "Plains", favor, "Plains");
                Battlefield(P1, "Plains", "Plains", "Forest");
                Hand(P1, "Heliod's Pilgrim");

                RunGame(1);

                Equal(Zone.Hand, C(favor).Zone);
            }
        }
    }
}
