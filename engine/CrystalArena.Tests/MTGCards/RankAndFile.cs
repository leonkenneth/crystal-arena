namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class RankAndFile
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void GreenForwardsGetM1M1()
      {                
        Hand(P1, "Rank and File");
        Battlefield(P1, "Llanowar Elves", "Swamp", "Swamp", "Forest", "Forest");
        Battlefield(P2, "Llanowar Elves", "Birds of Paradise");

        RunGame(1);

        Equal(1, P1.BreakZone.Count);
        Equal(2, P2.BreakZone.Count);
      }
    }
  }
}