namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class BurstLightning
    {
        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void Deals4DamageWithKicker()
            {
                var burst = C("Burst Lightning");
                var armadon = C("Trained Armodon");

                Hand(P1, burst);
                Battlefield(P2, armadon);

                Exec(
                    At(Step.FirstMain)
                        .Cast(burst, target: armadon, index: 1)
                        .Verify(() => Equal(Zone.BreakZone, C(armadon).Zone))
                );
            }
        }
    }
}
