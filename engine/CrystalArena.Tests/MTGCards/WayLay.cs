namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class WayLay
    {
        public class PredefinedAi : PredefinedAiScenario
        {
            [Fact(Skip = "Old card")]
            public void SummonBlockers()
            {
                var thrun = C("Thrun, the Last Troll");
                var waylay = C("WayLay");

                Battlefield(P1, thrun);
                Battlefield(P2, "Plains", "Plains", "Plains");
                Hand(P2, waylay);

                Exec(
                    At(Step.DeclareAttackers).DeclareAttackers(thrun),
                    At(Step.SecondMain)
                        .Verify(() =>
                        {
                            Equal(Zone.BreakZone, C(thrun).Zone);
                            Equal(1, P2.Battlefield.Forwards.Count());
                        }),
                    At(Step.FirstMain, 2).Verify(() => Equal(0, P2.Battlefield.Forwards.Count()))
                );
            }
        }
    }
}
