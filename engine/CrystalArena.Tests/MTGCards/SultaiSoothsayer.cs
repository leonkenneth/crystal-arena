namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class SultaiSoothsayer
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PutForceInHandOtherIntoBreakZone()
            {
                var force = C("Verdant Force");

                Hand(P1, "Sultai Soothsayer");
                MainDeck(P1, "Forest", "Forest", "Forest", force);
                Battlefield(
                    P1,
                    "Swamp",
                    "Island",
                    "Island",
                    "Island",
                    "Forest",
                    "Forest",
                    "Forest",
                    "Forest"
                );

                RunGame(1);

                Equal(Zone.Hand, C(force).Zone);
                Equal(3, P1.BreakZone.Count);
            }
        }
    }
}
