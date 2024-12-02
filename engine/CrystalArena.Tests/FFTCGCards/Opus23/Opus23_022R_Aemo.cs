namespace CrystalArena.Tests.FFTCGCards.Opus23
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Opus23_022R_Aemo : PredefinedScenario
  {
    [Fact]
    public void OpponentRemoveTheirHandFromTheGameUntilEot()
    {
      var aemo = C("23-022R");
      Battlefield(P1, aemo);
      Hand(P2, "0-001X", "0-001X", "0-001X");

      Exec(
        At(Step.FirstMain, turn: 1)
           .Verify(() =>
          {
            Equal(3, P2.Hand.Count);
          })
          .Activate(aemo)
          .Verify(() =>
          {
            Equal(0, P2.Hand.Count);
          }),
        At(Step.FirstMain, turn: 2)
          .Verify(() =>
          {
            // 3 + 2 drawn cards
            Equal(5, P2.Hand.Count);
          })
      );
    }
  }
}