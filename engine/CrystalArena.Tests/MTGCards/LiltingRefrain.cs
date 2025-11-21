namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class LiltingRefrain
    {
        public class PredefinedAi : PredefinedAiScenario
        {
            [Fact(Skip = "Old card")]
            public void CounterDragon()
            {
                var dragon = C("Shivan Dragon");

                Hand(P1, dragon);
                Battlefield(P2, "Lilting Refrain");

                Exec(
                    At(Step.FirstMain, 3)
                        .Cast(dragon)
                        .Verify(() => Equal(Zone.BreakZone, C(dragon).Zone))
                );
            }
        }
    }
}
