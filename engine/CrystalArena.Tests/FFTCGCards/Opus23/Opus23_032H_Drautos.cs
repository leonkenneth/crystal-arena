namespace CrystalArena.Tests.FFTCGCards.Opus23
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Opus23_032H_Drautos : PredefinedScenario
  {
    [Fact]
    public void DamageProtection()
    {
      var drautos = C("23-032H");
      var testDeal20000Summon1 = C("0-005X");
      var testDeal20000Summon2 = C("0-005X");
      Hand(P1, testDeal20000Summon1);
      Battlefield(P1, drautos);
      Hand(P2, testDeal20000Summon2);

      Exec(
        At(Step.FirstMain, turn: 1)
          .Cast(testDeal20000Summon2, target: drautos)
          .Verify(() =>
          {
            Equal(0, drautos.Card.Damage);
            True(drautos.Card.Zone == Zone.Battlefield);
          })
          .Cast(testDeal20000Summon1, target: drautos)
          .Verify(() =>
          {
            True(drautos.Card.Zone == Zone.BreakZone);
          })
      );
    }
  }
}