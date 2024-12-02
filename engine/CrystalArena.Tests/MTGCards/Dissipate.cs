namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Dissipate
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void CounterAxeAndRemoveFromPlayIt()
      {
        Hand(P1, "Lava Axe");
        Battlefield(P1, "Mountain", "Mountain", "Forest", "Forest", "Forest");

        P2.Life = 5;
        Hand(P2, "Dissipate");
        Battlefield(P2, "Island", "Island", "Forest", "Forest", "Forest");

        RunGame(1);

        Equal(0, P1.BreakZone.Count);
        Equal(1, P1.RemovedFromPlay.Count());
      }
    }
  }
}
