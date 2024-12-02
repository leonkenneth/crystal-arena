namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class BraidwoodSextant
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void SearchForBackup()
      {
        Battlefield(P1, "Braidwood Sextant", "Forest", "Forest");
        MainDeck(P1, "Forest");

        RunGame(1);

        Equal(3, P1.Battlefield.Backups.Count());
      }
    }
  }
}