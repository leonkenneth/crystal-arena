namespace CrystalArena.Tests.FFTCGCards.Opus23
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Opus23_015C_Notsugo : PredefinedScenario
  {
    [Fact]
    public void Deal9000()
    {
      var testForward = C("0-002X");
      var notsugo = C("23-015C");
      var anotherMonster = C("0-003X");
      Battlefield(P2, testForward);
      Battlefield(P1, notsugo, anotherMonster);

      Exec(
        At(Step.FirstMain, turn: 1)
          .Activate(notsugo, costTarget: anotherMonster, target: testForward),
        At(Step.SecondMain, turn: 1)
          .Verify(() =>
          {
            True(testForward.Card.Zone() == Zone.BreakZone);
            True(notsugo.Card.Zone() == Zone.BreakZone);
            True(anotherMonster.Card.Zone() == Zone.BreakZone);
          })
      );
    }
  }
}