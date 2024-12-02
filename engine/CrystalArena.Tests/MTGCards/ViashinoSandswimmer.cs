namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ViashinoSandswimmer
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void GoesToHandOrToBreakZone()
      {
        var sandswimmer = C("Viashino Sandswimmer");
        
        Hand(P2, "Shock");        
        Battlefield(P1, sandswimmer, "Mountain");
        Battlefield(P2, "Mountain");

        RunGame(1);

        True(Zone.BreakZone == C(sandswimmer).Zone || Zone.Hand == C(sandswimmer).Zone);        
      }
    }
  }
}