namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class AvalancheRiders
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyBackupDeal2Damage()
      {
        Hand(P1, "Avalanche Riders");
        Battlefield(P1, "Mountain", "Mountain", "Forest", "Forest");
        Battlefield(P2, "Mountain");
        
        RunGame(1);

        Equal(0, P2.Battlefield.Count(c => c.Is().Backup));
        Equal(18, P2.Life);
      }
    }
  }
}