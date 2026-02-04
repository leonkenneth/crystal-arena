namespace CrystalArena.Tests.FFTCGCards.Opus23
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Test_005_Deal20000Summon
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void ShouldNotBeCastableFromBreakzone()
            {
                var testSummonInHand = C("0-005X");
                var testSummonInBreakzone = C("0-005X");

                Hand(P1, testSummonInHand);
                BreakZone(P1, testSummonInBreakzone);

                // Without another Kain in hand, the ability cannot be activated
                Exec(
                    At(Step.FirstMain, turn: 1)
                        .Verify(() =>
                        {
                            Equal(1, testSummonInHand.Card.CanCast().Count);
                            Equal(0, testSummonInBreakzone.Card.CanCast().Count);
                        })
                );
            }
        }
    }
}
