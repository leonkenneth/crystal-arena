namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class GrimContest
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void FightWallandBear()
      {        
        Hand(P1, "Grim Contest");
        Battlefield(P1, "Grizzly Bears", "Wall of Frost", "Forest", "Forest", "Swamp");        
        Battlefield(P2, "Grizzly Bears", "Forest", "Forest", "Swamp");

        P2.Life = 2;
        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}
