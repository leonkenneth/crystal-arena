namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class SkitteringSkirge
    {
        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void SkirgeDiesAfterPlayingBear()
            {
                var skirge = C("Skittering Skirge");
                var bear = C("Grizzly Bears");

                Battlefield(P1, skirge);
                Hand(P1, bear);

                Exec(
                    At(Step.FirstMain).Cast(bear),
                    At(Step.SecondMain).Verify(() => Equal(Zone.BreakZone, C(skirge).Zone))
                );
            }
        }
    }
}
