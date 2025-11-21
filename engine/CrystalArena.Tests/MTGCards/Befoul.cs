namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Befoul
    {
        public class PredefinedAi : PredefinedAiScenario
        {
            [Fact(Skip = "Old card")]
            public void CannotBeRegenerated()
            {
                var befoul = C("Befoul");
                var troll = C("Albino Troll");

                Hand(P1, befoul);
                Battlefield(P2, "Forest", "Forest", troll);

                Exec(
                    At(Step.FirstMain).Cast(befoul, target: troll),
                    At(Step.SecondMain).Verify(() => Equal(Zone.BreakZone, C(troll).Zone))
                );
            }
        }
    }
}
