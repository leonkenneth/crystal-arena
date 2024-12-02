namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ArchfiendOfDepravity
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void PlayerSelectsTwoForwardsAndSacrificeRest()
      {
        Battlefield(P1, "Archfiend of Depravity", "Wall of Frost", "Grizzly Bears", "Forest", "Forest", "Forest");

        Battlefield(P2, "Grizzly Bears", "Wall of Frost", "Grizzly Bears");

        RunGame(2);

        Equal(0, P1.BreakZone.Forwards.Count());
        Equal(1, P2.BreakZone.Forwards.Count());
      }
    }
  }
}
