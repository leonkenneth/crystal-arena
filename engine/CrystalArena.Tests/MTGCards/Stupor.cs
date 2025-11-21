namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Stupor
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PlayStuporIfOpponentHasAtLeast2CardsInHand()
            {
                Battlefield(P1, "Swamp", "Swamp", "Swamp");
                Hand(P1, "Stupor");
                Hand(P2, "Swamp", "Swamp");

                RunGame(maxTurnCount: 2);

                Equal(2, P2.BreakZone.Count());
            }
        }

        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void OpponentDiscardsCardAtRandomThenDiscardsACard()
            {
                var stupor = C("Stupor");
                Hand(P1, stupor);
                Hand(P2, C("Forest"), C("Forest"));

                Exec(At(Step.FirstMain).Cast(stupor).Verify(() => Equal(0, P2.Hand.Count())));
            }
        }
    }
}
