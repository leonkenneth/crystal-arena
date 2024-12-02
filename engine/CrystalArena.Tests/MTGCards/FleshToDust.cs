namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class FleshToDust
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyDragon()
      {
        Hand(P1, "Flesh To Dust");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Trained Armodon");
        Battlefield(P2, "Shivan Dragon");

        P2.Life = 3;
                
        RunGame(1);
        
        Equal(0, P2.Life);
      }
    }
  }
}