namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class JeskaiCharm
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void Deal4DamageToOpponent()
      {
        Hand(P1, "Jeskai Charm");
        Battlefield(P1, "Island", "Mountain", "Plains");
        P2.Life = 4;

        RunGame(1);

        Assert.Equal(0, P2.Life);
      }

      [Fact (Skip = "Old card")]
      public void PumpForwards()
      {
        Hand(P1, "Jeskai Charm");
        Battlefield(P1, "Island", "Mountain", "Plains", "Grizzly Bears", "Grizzly Bears", 
          "Grizzly Bears", "Grizzly Bears");
        
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        P2.Life = 10;

        RunGame(1);

        Assert.Equal(0, P1.BreakZone.Forwards.Count());
        Assert.Equal(3, P2.BreakZone.Forwards.Count());
      }
    }
  }
}