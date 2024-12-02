namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class WallOfEssence
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void BlockWarMachingeAndGainLife()
      {
        Battlefield(P1, "Thran War Machine", "Forest", "Forest", "Forest", "Forest");        
        Battlefield(P2, "Wall of Essence");
        P2.Life = 1;

        RunGame(1);

        Equal(5, P2.Life);        
      }
    }
  }
}