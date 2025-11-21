namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class FiresOfYavimaya
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void FiresOfYavimayaGivesBearHaste()
            {
                var bear = C("Grizzly Bears");
                Battlefield(P1, "Forest", "Forest", "Fires of Yavimaya");
                Hand(P1, bear);

                RunGame(maxTurnCount: 2);

                Equal(Zone.Battlefield, C(bear).Zone);
                True(C(bear).Has().Haste);
                Equal(18, P2.Life);
            }
        }

        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void ForwardsHaveHaste()
            {
                var fires = C("Fires of Yavimaya");
                var bear = C("Grizzly Bears");

                Hand(P1, fires);
                Battlefield(P1, bear);

                Exec(
                    At(Step.FirstMain)
                        .Cast(fires)
                        .Verify(() =>
                        {
                            Equal(Zone.Battlefield, C(fires).Zone);
                            True(C(bear).Has().Haste);
                        })
                );
            }

            [Fact(Skip = "Old card")]
            public void SacToGiveTargetPlus2Plus2()
            {
                var fires = C("Fires of Yavimaya");
                var bear = C("Grizzly Bears");

                Battlefield(P1, fires, bear);

                Exec(
                    At(Step.FirstMain)
                        .Activate(fires, target: bear)
                        .Verify(() =>
                        {
                            Equal(4, C(bear).Power);
                            Equal(4, C(bear).Toughness);
                            Equal(Zone.BreakZone, C(fires).Zone);
                            False(C(bear).Has().Haste);
                        })
                );
            }

            [Fact(Skip = "Old card")]
            public void WhenForwardGoesToBreakZoneItNoLongerHasHaste()
            {
                var fires = C("Fires of Yavimaya");
                var bear = C("Grizzly Bears");
                var shock = C("Shock");

                Hand(P1, fires);
                Battlefield(P1, bear);
                Hand(P2, shock);

                Exec(
                    At(Step.FirstMain)
                        .Cast(fires)
                        .Verify(() =>
                        {
                            Equal(Zone.Battlefield, C(fires).Zone);
                            True(C(bear).Has().Haste);
                        }),
                    At(Step.SecondMain)
                        .Cast(shock, target: bear)
                        .Verify(() => False(C(bear).Has().Haste))
                );
            }
        }
    }
}
