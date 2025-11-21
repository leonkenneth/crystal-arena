namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class BoonweaverGiant
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void SearchMainDeck()
            {
                var giant = C("Boonweaver Giant");
                var rancor = C("Rancor");

                Hand(P1, giant);
                Battlefield(
                    P1,
                    "Plains",
                    "Plains",
                    "Plains",
                    "Plains",
                    "Plains",
                    "Plains",
                    "Plains",
                    "Plains"
                );
                MainDeck(P1, rancor);

                RunGame(1);

                Equal(C(giant), C(rancor).AttachedTo);
            }

            [Fact(Skip = "Old card")]
            public void SearchBreakZone()
            {
                var giant = C("Boonweaver Giant");
                var rancor = C("Rancor");

                Hand(P1, giant);
                Battlefield(
                    P1,
                    "Plains",
                    "Plains",
                    "Plains",
                    "Plains",
                    "Plains",
                    "Plains",
                    "Plains",
                    "Plains"
                );
                BreakZone(P1, rancor);

                RunGame(1);

                Equal(C(giant), C(rancor).AttachedTo);
            }
        }
    }
}
