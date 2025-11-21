namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class EliteArchers
    {
        public class PredefinedAi : PredefinedAiScenario
        {
            [Fact(Skip = "Old card")]
            public void KillArmodon()
            {
                var armodon = C("Trained Armodon");

                Battlefield(P1, armodon);
                Battlefield(P2, "Elite Archers");

                Exec(
                    At(Step.DeclareAttackers).DeclareAttackers(armodon),
                    At(Step.SecondMain).Verify(() => Equal(Zone.BreakZone, C(armodon).Zone))
                );
            }
        }
    }
}
