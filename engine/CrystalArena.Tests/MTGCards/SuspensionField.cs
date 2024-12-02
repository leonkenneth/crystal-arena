namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SuspensionField
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void RemoveFromPlayDragon()
      {
        Hand(P1, "Suspension Field");
        Battlefield(P1, "Plains", "Plains");

        Battlefield(P2, "Shivan Dragon");

        RunGame(1);

        Equal(0, P2.Battlefield.Count);
      }

      [Fact (Skip = "Old card")]
      public void ReturnDragonFromRemoveFromPlay()
      {
        Hand(P1, "Suspension Field");
        Battlefield(P1, "Plains", "Plains");

        Hand(P2, "Naturalize");
        Battlefield(P2, "Shivan Dragon", "Forest", "Plains");

        RunGame(2);

        Equal(3, P2.Battlefield.Count);
        Equal(1, P1.BreakZone.Count);
      }
    }
  }
}
