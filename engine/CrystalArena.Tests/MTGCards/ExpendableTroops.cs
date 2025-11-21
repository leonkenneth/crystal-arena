namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class ExpendableTroops
    {
        public class PredefinedAi : PredefinedAiScenario
        {
            [Fact(Skip = "Old card")]
            public void KillSomnophore()
            {
                var somnophore = C("Somnophore");
                var troops = C("Expendable Troops");

                Battlefield(P1, somnophore);
                Battlefield(P2, troops);

                Exec(
                    At(Step.DeclareAttackers).DeclareAttackers(somnophore),
                    At(Step.SecondMain)
                        .Verify(() =>
                        {
                            Equal(Zone.BreakZone, C(somnophore).Zone);
                            Equal(Zone.BreakZone, C(troops).Zone);
                        })
                );
            }
        }
    }
}
