namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Rewind
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void CounterForce()
      {
        Hand(P2, "Rewind");
        Hand(P1, "Verdant Force");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");
        Battlefield(P2, "Island", "Island", "Island", "Island");

        RunGame(1);

        Equal(4, P2.Battlefield.Count(x => x.Is().Backup && !x.IsTapped));
        Equal(1, P1.BreakZone.Count);
        Equal(1, P2.BreakZone.Count);
      }

      [Fact (Skip = "Old card")]
      public void DoNotUntapBackupsIfCountered()
      {
        Hand(P2, "Rewind");
        Hand(P1, "Verdant Force", "Mana Leak");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Island",
          "Island");
        Battlefield(P2, "Island", "Island", "Island", "Island");

        RunGame(1);

        Equal(4, P2.Battlefield.Count(x => x.Is().Backup && x.IsTapped));
        Equal(1, P1.BreakZone.Count);
        Equal(1, P2.BreakZone.Count);
      }
    }
  }
}