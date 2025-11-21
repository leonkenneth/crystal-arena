namespace CrystalArena.Tests.FFTCGCards.Opus23
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Opus23_011L_Terra : PredefinedScenario
    {
        [Fact]
        public void BreakZoneProtectionNotEnabled()
        {
            var testInvocation = C("0-004X");
            var cardInBreakZone = C("0-004X");
            BreakZone(P2, cardInBreakZone);
            Hand(P1, testInvocation);

            Exec(
                At(Step.FirstMain, turn: 1)
                    .Cast(testInvocation, cardInBreakZone)
                    .Verify(() =>
                    {
                        True(cardInBreakZone.Card.Zone() == Zone.RemovedFromPlay);
                    })
            );
        }

        [Fact]
        public void BreakZoneProtectionEnabled()
        {
            var terra = C("23-011L");
            var testInvocation = C("0-004X");
            var cardInBreakZone = C("0-004X");
            BreakZone(P2, cardInBreakZone);
            Battlefield(P2, terra);
            Hand(P1, testInvocation);

            Exec(
                At(Step.FirstMain, turn: 1)
                    .Cast(testInvocation, cardInBreakZone)
                    .Verify(() =>
                    {
                        True(cardInBreakZone.Card.Zone() == Zone.BreakZone);
                    })
            );
        }
    }
}
