namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Retaliation
    {
        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void PumpBearTwice()
            {
                var bear1 = C("Grizzly Bears");
                var bear2 = C("Grizzly Bears");
                var elves = C("Llanowar Elves");

                Battlefield(P1, bear1, "Retaliation");
                Battlefield(P2, bear2, elves);

                Exec(
                    At(Step.DeclareAttackers).DeclareAttackers(bear1),
                    At(Step.DeclareBlocker).DeclareBlockers(bear1, bear2, bear1, elves),
                    At(Step.SecondMain)
                        .Verify(() =>
                        {
                            Equal(4, C(bear1).Power);
                            Equal(4, C(bear1).Toughness);
                        })
                );
            }

            [Fact(Skip = "Old card")]
            public void DoNotPump()
            {
                var bear1 = C("Grizzly Bears");
                var bear2 = C("Grizzly Bears");

                Battlefield(P1, bear1);
                Battlefield(P2, bear2, "Retaliation");

                Exec(
                    At(Step.DeclareAttackers).DeclareAttackers(bear1),
                    At(Step.DeclareBlocker).DeclareBlockers(bear1, bear2),
                    At(Step.SecondMain)
                        .Verify(() =>
                        {
                            Equal(Zone.BreakZone, C(bear2).Zone);
                        })
                );
            }
        }

        public class PredefinedAi : PredefinedAiScenario
        {
            [Fact(Skip = "Old card")]
            public void PumpOnce()
            {
                var treefolk = C("Blanchwood Treefolk");
                var swine = C("Argothian Swine");

                Hand(P1, treefolk);
                Battlefield(P1, treefolk, "Retaliation", "Titania's Chosen");
                Battlefield(P2, swine);

                P2.Life = 4;

                Exec(
                    At(Step.FirstMain).Cast(treefolk),
                    At(Step.DeclareAttackers, turn: 3).DeclareAttackers(treefolk),
                    At(Step.SecondMain, turn: 3)
                        .Verify(() =>
                        {
                            Equal(5, C(treefolk).Power);
                        })
                );
            }
        }
    }
}
