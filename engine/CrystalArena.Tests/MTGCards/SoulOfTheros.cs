namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class SoulOfTheros
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void ActivateFromBattlefield()
      {
        Battlefield(P1, "Soul Of Theros", "Grizzly Bears", "Forest", 
          "Forest", "Forest", "Plains", "Plains", "Plains");

        RunGame(1);

        Equal(8, P2.Life);
      }

      [Fact (Skip = "Old card")]
      public void ActivateFromBreakZone()
      {
        var theros = C("Soul Of Theros");
        Battlefield(P1, "Grizzly Bears", "Forest", "Forest", "Forest", "Plains", "Plains", "Plains");        
        BreakZone(P1, theros);

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
        Equal(Zone.RemovedFromPlay, C(theros).Zone);        
      }
    }
  }
}
