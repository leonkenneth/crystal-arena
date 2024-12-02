namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  class RotfeasterMaggot
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void GainLifeEqualToDragonToughness()
      {
        Hand(P1, "Rotfeaster Maggot");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        BreakZone(P1, "Shivan Dragon");

        BreakZone(P2, "Grizzly Bears");

        RunGame(1);

        Equal(25, P1.Life);
      }

      [Fact (Skip = "Old card")]
      public void CannotRemoveFromPlayDragon()
      {
        Hand(P1, "Rotfeaster Maggot");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        BreakZone(P1, "Shivan Dragon");

        Battlefield(P2, "Tormod's Crypt");

        RunGame(1);

      }
    }
  }
}
