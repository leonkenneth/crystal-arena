namespace CrystalArena.Tests.FFTCGCards.Opus23
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Opus23_014H_Nelapa : PredefinedAiScenario
  {
    [Fact]
    public void DoesNotTriggerFromHand()
    {
      var nelapa = C("23-014H");
      var basicForward = C("0-002X");
      var summon = C("0-005X");
      Hand(P1, summon);
      Hand(P2, nelapa);
      Battlefield(P2, basicForward);

      Exec(
        At(Step.FirstMain, turn: 1)
          .Cast(summon, target: basicForward),
        At(Step.SecondMain, turn: 1)
          .Verify(() =>
          {
            Equal(7, P1.Life);
          })
      );
    }
    
    [Fact]
    public void TriggersOnField()
    {
      var nelapa = C("23-014H");
      var basicForward = C("0-002X");
      var summon = C("0-005X");
      Hand(P1, summon);
      Battlefield(P2, basicForward, nelapa);

      Exec(
        At(Step.FirstMain, turn: 1)
          .Cast(summon, target: basicForward),
        At(Step.SecondMain, turn: 1)
          .Verify(() =>
          {
            Equal(6, P1.Life);
          })
      );
    }
  }
}