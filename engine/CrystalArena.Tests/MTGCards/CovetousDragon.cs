namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class CovetousDragon
    {
        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void SacWhenLastArtifactIsDestroyed()
            {
                var dragon = C("Covetous Dragon");
                var disenchant = C("Disenchant");
                var dragonBlood = C("Dragon Blood");

                Battlefield(P1, dragonBlood, dragon);
                Hand(P2, disenchant);

                Exec(
                    At(Step.FirstMain)
                        .Cast(disenchant, target: dragonBlood)
                        .Verify(() => Equal(Zone.BreakZone, C(dragon).Zone))
                );
            }
        }
    }
}
