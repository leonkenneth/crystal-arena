namespace CrystalArena.Tests.FFTCGCards.Opus23
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Opus23_023H_Anima : PredefinedScenario
  {
    [Fact]
    public void DullAndFreezeAllOpponentCharacters()
    {
      var anima = C("23-023H");
      var backup = C("0-001X");
      Hand(P1, anima);
      Hand(P2);
      Battlefield(P2, backup);

      Exec(
        At(Step.FirstMain, turn: 1)
          .Verify(() =>
          {
            P1.Hand.Last().Discard();
            False(backup.Card.IsTapped);
            Equal(1, P1.Hand.Count());
            Equal(0, P2.Hand.Count());
          })
           .Cast(anima)
          .Verify(() =>
          {
            True(backup.Card.IsTapped);
          }),
        At(Step.FirstMain, turn: 2)
          .Verify(() =>
          {
            True(backup.Card.IsTapped);
          })
      );
    }
  }
}