namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class RavenFamiliar
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PutForceInHandOtherOnBottom()
            {
                var force = C("Verdant Force");

                Hand(P1, "Raven Familiar");
                MainDeck(P1, "Forest", "Forest", force);
                Battlefield(
                    P1,
                    "Island",
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
                Equal(
                    new[] { "Forest", "Forest" },
                    P1.MainDeck.Reverse().Take(2).Select(x => x.Name).ToArray()
                );
            }
        }
    }
}
