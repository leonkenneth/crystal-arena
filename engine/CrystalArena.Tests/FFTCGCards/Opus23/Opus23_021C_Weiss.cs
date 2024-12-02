namespace CrystalArena.Tests.FFTCGCards.Opus23
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Opus23_021C_Weiss : PredefinedScenario
  {
    [Fact]
    public void GainsPowerIfMoreThan2Tsviets()
    {
      var weiss = C("23-021C");
      var shelke = C("23-025C");
      Battlefield(P1, weiss, shelke);

      Exec(
        At(Step.FirstMain, turn: 1)
           .Verify(() =>
          {
            Equal(9000, weiss.Card.Power);
          })
      );
    }
  }
}