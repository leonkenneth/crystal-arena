namespace CrystalArena.Tests.Cards
{
    using CrystalArena.AI;
    using Infrastructure;
    using Xunit;

    public class LilianaVess
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PutAllForwardsToBattlefield()
            {
                var liliana = C("Liliana Vess");

                Battlefield(P1, liliana.AddCounters(9, CounterType.Loyality));
                BreakZone(P1, "Shivan Dragon");
                BreakZone(P2, "Shivan Dragon");

                P2.Life = 10;

                RunGame(3);

                Equal(0, P2.Life);
            }

            [Fact(Skip = "Old card")]
            public void SearchMainDeckForCorrupt()
            {
                var liliana = C("Liliana Vess");

                Hand(P1, "Sign in Blood");

                Battlefield(
                    P1,
                    liliana.AddCounters(5, CounterType.Loyality),
                    "Swamp",
                    "Swamp",
                    "Swamp",
                    "Swamp",
                    "Swamp",
                    "Swamp",
                    "Swamp",
                    "Swamp"
                );

                MainDeck(P1, "Swamp", "Swamp", "Swamp", "Corrupt");

                P2.Life = 8;

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
