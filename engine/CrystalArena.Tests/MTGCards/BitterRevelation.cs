namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class BitterRevelation
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PutForcesInHandOtherIntoBreakZone()
            {
                var force1 = C("Verdant Force");
                var force2 = C("Verdant Force");

                Hand(P1, "Bitter Revelation");
                MainDeck(P1, "Forest", "Forest", force2, force1);
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

                Equal(18, P1.Life);
                Equal(Zone.Hand, C(force1).Zone);
                Equal(Zone.Hand, C(force2).Zone);
                Equal(
                    new[] { "Forest", "Forest" },
                    P1.BreakZone.Take(2).Select(x => x.Name).ToArray()
                );
            }
        }
    }
}
