namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class GreaterGood
    {
        public class PredefinedAi : PredefinedAiScenario
        {
            [Fact(Skip = "Old card")]
            public void SacForce()
            {
                var force = C("Verdant Force");
                var blade = C("Doom blade");

                Battlefield(P2, force, "Greater Good");
                Hand(P1, blade);

                Exec(
                    At(Step.FirstMain).Cast(blade, target: force),
                    At(Step.SecondMain)
                        .Verify(() =>
                        {
                            Equal(Zone.BreakZone, C(force).Zone);
                            Equal(4, P2.Hand.Count);
                        })
                );
            }
        }
    }
}
