namespace CrystalArena.Tests.FFTCGCards.Opus23
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Opus23_016R_Bahamut : PredefinedScenario
    {
        [Fact]
        public void RemoveFromPlayTargetsWhenRequirementsAreMet()
        {
            var testForwardInBattlefield = C("0-002X");
            var testForwardInBreakZone = C("0-002X");
            var bahamut = C("23-016R");
            Battlefield(P2, testForwardInBattlefield);
            BreakZone(P2, testForwardInBreakZone);
            Hand(P1, bahamut);

            Exec(
                At(Step.FirstMain, turn: 1)
                    .Cast(bahamut, targets: Ts(testForwardInBattlefield, testForwardInBreakZone)),
                At(Step.SecondMain, turn: 1)
                    .Verify(() =>
                    {
                        True(testForwardInBattlefield.Card.Zone() == Zone.RemovedFromPlay);
                        True(testForwardInBreakZone.Card.Zone() == Zone.RemovedFromPlay);
                    })
            );
        }
    }
}
