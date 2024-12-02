namespace CrystalArena.Tests.Cards
{
  using System.Collections.Generic;
  using Infrastructure;
  using Xunit;

  public class SoulOfShandalar
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DealDamageToPlayerAndForward()
      {
        var armodon = C("Trained Armodon");

        Battlefield(P1, "Soul of Shandalar", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");        
        Battlefield(P2, armodon);
        
        P2.Life = 9;

        RunGame(1);

        Equal(Zone.BreakZone, C(armodon).Zone);
        Equal(0, P2.Life);
      }

      [Fact (Skip = "Old card")]
      public void DealDamageToPlayerOnly()
      {        
        Battlefield(P1, "Soul of Shandalar", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        
        P2.Life = 9;

        RunGame(2);        
        Equal(0, P2.Life);
      }
    }        
  }
}