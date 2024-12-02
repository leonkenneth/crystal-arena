namespace CrystalArena.Tests.FFTCGCards.Opus23
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Opus23_025C_Shelke : PredefinedScenario
  {
    [Fact]
    public void GainCrystalTriggersOnlyOncePerTurn()
    {
      var weiss = C("23-021C");
      var shelke = C("23-025C");
      Hand(P1, weiss, shelke);

      Exec(
        At(Step.FirstMain, turn: 1)
           .Verify(() =>
          {
            False(P1.HasMana("{Z}".Parse()));
          })
          .Cast(shelke)
          .Verify(() =>
          {
            True(P1.HasMana("{Z}".Parse()));
            False(P1.HasMana("{Z}{Z}".Parse()));
          })
          .Cast(weiss)
          .Verify(() =>
          {
            // Weiss AI enters the field consumes the crystal
            False(P1.HasMana("{Z}".Parse()));
            False(P1.HasMana("{Z}{Z}".Parse()));
          })
      );
    }
  }
}