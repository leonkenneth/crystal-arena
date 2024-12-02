namespace CrystalArena.Tests.FFTCGCards.Opus23
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Opus23_004R_Kefka : PredefinedScenario
  {
    [Fact]
    public void SimpleDamageAtStart()
    {
      var kefka = C("23-004R");
      Battlefield(P1, kefka);

      Exec(
        At(Step.DeclareAttackers, turn: 1)
          .DeclareAttackers(kefka),
        At(Step.EndOfCombat, turn: 1)
          .Verify(() =>
          {
            Equal(6, P2.Life);
          })
      );
    }
    
    [Fact]
    public void DoubleDamageAtDamage5()
    {
      var kefka = C("23-004R");
      P1.Life = 2;
      Battlefield(P1, kefka);

      Exec(
        At(Step.DeclareAttackers, turn: 1)
          .DeclareAttackers(kefka),
          At(Step.EndOfCombat, turn: 1)
            .Verify(() =>
          {
            Equal(5, P2.Life);
          })
      );
    }
  }
}