namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ViashinoHeretic
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroySword()
      {
        var sword = C("Sword of Fire and Ice");

        Battlefield(P1, "Viashino Heretic", "Mountain", "Mountain");        
        Battlefield(P2, sword);

        RunGame(2);

        Equal(17, P2.Life);
        Equal(Zone.BreakZone, C(sword).Zone);
      }
    }
  }
}