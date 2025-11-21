namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class DragonBlood
    {
        public class PredefinedAi : PredefinedAiScenario
        {
            [Fact(Skip = "Old card")]
            public void PumpBeforeDestroyed()
            {
                var bear = C("Grizzly Bears");
                var blood = C("Dragon Blood");
                var disenchant = C("Disenchant");

                Hand(P1, disenchant);
                Battlefield(P2, bear, "Plains", "Plains", "Plains", blood);

                Exec(
                    At(Step.FirstMain)
                        .Cast(disenchant, target: blood)
                        .Verify(() => Equal(3, C(bear).Power))
                );
            }

            [Fact(Skip = "Old card")]
            public void PumpEot()
            {
                var songstitcher = C("Songstitcher");

                Battlefield(P2, "Plains", "Plains", "Dragon Blood", "Plains", songstitcher);

                Exec(At(Step.Upkeep, 2).Verify(() => Equal(1, C(songstitcher).Counters)));
            }

            public class Ai : AiScenario
            {
                [Fact(Skip = "Old card")]
                public void PumpUnblockedForward()
                {
                    var blood = C("Dragon Blood");
                    var bear = C("Grizzly Bears");

                    Battlefield(P1, blood, bear, "Plains", "Plains", "Plains");
                    RunGame(1);
                    Equal(17, P2.Life);
                }
            }
        }
    }
}
