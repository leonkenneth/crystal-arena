namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class DiscipleOfGrace
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Cycle()
            {
                var dragon = C("Shivan Dragon");
                var disciple = C("Disciple of Grace");

                Hand(P1, disciple);
                Battlefield(P1, "Plains", "Plains");
                Battlefield(P2, dragon);

                RunGame(2);

                Equal(Zone.BreakZone, C(disciple).Zone);
                Equal(1, P1.Hand.Count);
            }

            [Fact(Skip = "Old card")]
            public void Cast()
            {
                var titan = C("Grave Titan");
                var disciple = C("Disciple of Grace");

                Hand(P1, disciple);
                Battlefield(P1, "Plains", "Plains");
                Battlefield(P2, titan);

                RunGame(2);

                Equal(Zone.Battlefield, C(disciple).Zone);
                Equal(20, P1.Life);
            }
        }

        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void Cycle()
            {
                var disciple = C("Disciple of Grace");
                Hand(P1, disciple);

                Exec(
                    At(Step.FirstMain)
                        .Cycle(disciple)
                        .Verify(() =>
                        {
                            Equal(Zone.BreakZone, C(disciple).Zone);
                            Equal(1, P1.Hand.Count);
                        })
                );
            }
        }
    }
}
