namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Gamble
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void SearchForForce()
            {
                var force = C("Verdant Force");

                MainDeck(P1, "Forest", "Forest", "Forest", force);
                Hand(P1, "Gamble", "Forest", "Forest", "Forest", "Forest", "Forest");
                Battlefield(
                    P1,
                    "Mountain",
                    "Forest",
                    "Forest",
                    "Forest",
                    "Forest",
                    "Forest",
                    "Forest",
                    "Forest",
                    "Forest"
                );

                RunGame(1);

                True(C(force).Zone == Zone.Battlefield || C(force).Zone == Zone.BreakZone);
                Equal(2, P1.BreakZone.Count);
            }
        }
    }
}
