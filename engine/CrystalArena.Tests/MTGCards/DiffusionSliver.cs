namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class DiffusionSliver
    {
        public class PredefinedAi : PredefinedAiScenario
        {
            [Fact(Skip = "Old card")]
            public void EnoughManaToPayTheExtraCost()
            {
                var blade = C("Doom blade");
                var sliver = C("Diffusion Sliver");

                Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp");
                Hand(P1, blade);
                Battlefield(P2, sliver, "Diffusion Sliver");

                Exec(
                    At(Step.FirstMain).Cast(blade, target: sliver),
                    At(Step.SecondMain).Verify(() => Equal(Zone.BreakZone, C(sliver).Zone))
                );
            }

            [Fact(Skip = "Old card")]
            public void NotEnoughManaToPayTheExtraCost()
            {
                var blade = C("Doom blade");
                var sliver = C("Diffusion Sliver");

                Battlefield(P1, "Swamp", "Swamp", "Swamp");
                Hand(P1, blade);
                Battlefield(P2, sliver, "Diffusion Sliver");

                Exec(
                    At(Step.FirstMain).Cast(blade, target: sliver),
                    At(Step.SecondMain).Verify(() => Equal(Zone.Battlefield, C(sliver).Zone))
                );
            }
        }
    }
}
