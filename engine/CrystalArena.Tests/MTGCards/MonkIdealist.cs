namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class MonkIdealist
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void ReturnMonsterFromBreakZone()
            {
                var pariah = C("Pariah");
                var idealist = C("Monk Idealist");

                Hand(P1, idealist);
                BreakZone(P1, pariah);
                Battlefield(P1, "Plains", "Plains", "Plains");

                RunGame(1);

                Equal(Zone.Hand, C(pariah).Zone);
            }

            [Fact(Skip = "Old card")]
            public void DoNotCastIdealistWithNoMonstersInYourBreakZone()
            {
                var idealist = C("Monk Idealist");

                Hand(P1, idealist);
                Battlefield(P1, "Plains", "Plains", "Plains");

                RunGame(1);

                Equal(Zone.Hand, C(idealist).Zone);
            }
        }
    }
}
