namespace CrystalArena.Tests.Cards
{
  using Xunit;
   using Infrastructure;

  public class ConstrictingSliver
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void PlaySliverRemoveFromPlayDragon()
      {
        var dragon = C("Shivan Dragon");

        Hand(P1, "Venom Sliver");        
        Battlefield(P1, "Constricting Sliver", "Forest", "Forest");                
        Battlefield(P2, dragon);

        RunGame(1);
        
        Equal(Zone.RemovedFromPlay, C(dragon).Zone);
      }

      [Fact (Skip = "Old card")]
      public void PlaySelfRemoveFromPlayDragon()
      {
        var dragon = C("Shivan Dragon");
        var sliver = C("Constricting Sliver");

        Hand(P1, sliver);
        Battlefield(P1, "Plains", "Forest", "Forest", "Plains", "Forest", "Forest");
        Battlefield(P2, dragon);

        RunGame(1);

        Equal(Zone.Battlefield, C(sliver).Zone);
        Equal(Zone.RemovedFromPlay, C(dragon).Zone);
      }

      [Fact (Skip = "Old card")]
      public void ReturnDragonToPlay()
      {
        var dragon = C("Shivan Dragon");
        var sliver = C("Venom Sliver");

        Hand(P1, sliver);
        Battlefield(P1, "Constricting Sliver", "Forest", "Forest");
        
        Hand(P2, "Shock");
        Battlefield(P2, dragon, "Mountain");

        RunGame(2);

        Equal(Zone.BreakZone, C(sliver).Zone);
        Equal(Zone.Battlefield, C(dragon).Zone);
      }
    }

  }
}