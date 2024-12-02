namespace CrystalArena.Tests.FFTCGCards.Opus23
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Opus23_006R_Soulcage : PredefinedScenario
  {
    [Fact]
    public void JustAMonster()
    {
      var soulcage = C("23-006R");
      Battlefield(P1, soulcage);

      Exec(
        At(Step.FirstMain, turn: 1)
          .Verify(() =>
          {
            Equal(0, P1.Battlefield.Count(x => x.Is().Forward));
          })
      );
    }
    
    [Fact]
    public void AlsoAForward()
    {
      var soulcage = C("23-006R");
      Battlefield(P1, soulcage, "0-003X");

      Exec(
        At(Step.FirstMain, turn: 1)
          .Verify(() =>
          {
            Equal(1, P1.Battlefield.Count(x => x.Is().Forward));
          })
      );
    }
  }
}