namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class CarrionBeetles
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void Remove2Cards()
      {
        BreakZone(P1, "Shivan Dragon", "Rancor");
        Battlefield(P2, "Carrion Beetles", "Swamp", "Swamp", "Swamp");

        RunGame(1);

        Equal(0, P1.Battlefield.Count());
      }
    }
  }
}