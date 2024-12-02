namespace CrystalArena.Tests.Scenarios
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class New
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AiShouldDiscardToGenerateManaIfNecessary()
      {
        Hand(P1, "0-001X", "0-002X", "0-003X", "0-004X", "0-001X");
        Hand(P2);

        RunGame(1);
        
        True(P1.Battlefield.Any());
      }
    }
  }
}