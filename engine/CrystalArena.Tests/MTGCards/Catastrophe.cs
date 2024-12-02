namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Catastrophe
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyForwards()
      {
        Hand(P1, "Catastrophe");

        Battlefield(P1, "Grizzly Bears", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");
        Battlefield(P2, "Grizzly Bears", "Shivan Dragon");

        P1.Life = 5;

        RunGame(2);

        Equal(0, P1.Battlefield.Forwards.Count());
        Equal(0, P2.Battlefield.Forwards.Count());
      }

      [Fact (Skip = "Old card")]
      public void DestroyBackups()
      {
        // todo make backups score dependable on other permanents count
        // so cpu will destroy all backups when he has better board position

        Hand(P1, "Catastrophe");

        Battlefield(P1, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");
        Battlefield(P2, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains",
          "Plains");

        RunGame(1);

        Equal(0, P1.Battlefield.Backups.Count());
        Equal(0, P2.Battlefield.Backups.Count());
      }
    }
  }
}