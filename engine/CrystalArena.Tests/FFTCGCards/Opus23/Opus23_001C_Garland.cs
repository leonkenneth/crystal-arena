namespace CrystalArena.Tests.FFTCGCards.Opus23
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Opus23_001C_Garland
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void LegendaryRule1()
            {
                var garland = C("23-001C");

                Hand(P1, garland);
                Battlefield(P2, "23-001C");

                Exec(
                    At(Step.FirstMain).Cast(garland),
                    At(Step.SecondMain)
                        .Verify(() =>
                        {
                            Equal(1, P1.Battlefield.Count());
                            Equal(1, P2.Battlefield.Count());
                        })
                );
            }

            [Fact]
            public void LegendaryRule2()
            {
                var garland = C("23-001C");

                Hand(P1, garland);
                Battlefield(P1, "23-001C");

                Exec(
                    At(Step.FirstMain).Cast(garland),
                    At(Step.SecondMain).Verify(() => Equal(0, P1.Battlefield.Count()))
                );
            }

            [Fact]
            public void GainControlOfForward()
            {
                var garland = C("23-001C");
                var opponentForward = C("Test Basic Forward");
                Hand(P1, garland, "23-001C");
                Battlefield(P2, opponentForward);

                Exec(
                    At(Step.FirstMain, turn: 1).Cast(garland).Target(opponentForward),
                    At(Step.SecondMain, turn: 1)
                        .Verify(() =>
                        {
                            Equal(P1, C(opponentForward).Controller);
                        })
                );
            }
        }
    }
}
