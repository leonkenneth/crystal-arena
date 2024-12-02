namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class BroodKeeper
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void RancorTheBroodKeeper()
      {
        Hand(P1, "Rancor");
        Battlefield(P1, "Brood Keeper", "Forest");

        RunGame(1);

        Equal(2, P1.Battlefield.Forwards.Count());
      }
    }
  }
}