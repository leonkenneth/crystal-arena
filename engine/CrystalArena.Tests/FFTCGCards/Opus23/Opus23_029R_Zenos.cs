namespace CrystalArena.Tests.FFTCGCards.Opus23
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Opus23_029R_Zenos : PredefinedScenario
  {
    [Fact]
    public void DiscardOnRemoveFromBZ()
    {
      var zenos = C("23-029R");
      var testRemoveFromGameSummon1 = C("0-004X");
      var testRemoveFromGameSummon2 = C("0-004X");
      var card1 = C("0-001X");
      var card2 = C("0-001X");
      Hand(P1, testRemoveFromGameSummon1, testRemoveFromGameSummon2);
      Battlefield(P1, zenos);
      Hand(P2, "0-001X", "0-001X");
      BreakZone(P2, card1, card2);

      Exec(
        At(Step.FirstMain, turn: 1)
          .Cast(testRemoveFromGameSummon1, target: card1)
          .Verify(() =>
          {
            Equal(1, P2.Hand.Count);
          })
          .Cast(testRemoveFromGameSummon2, target: card2)
          .Verify(() =>
          {
            Equal(1, P2.Hand.Count);
          })
      );
    }
  }
}