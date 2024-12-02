namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GoblinWelder
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void ExchangePitTrapAndEngine()
      {
        var trap = C("Pit Trap");
        var engine = C("Wurmcoil Engine");
        
        Battlefield(P1, trap, "Goblin Welder");        
        BreakZone(P1, engine);

        RunGame(1);

        Equal(Zone.Battlefield,C(engine).Zone);
        Equal(Zone.BreakZone,C(trap).Zone);
      }
    }
  }
}