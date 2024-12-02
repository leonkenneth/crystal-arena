namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GaeasCradle
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void P1Has3Mana()
      {
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears", "Gaea's Cradle");

        Equal(3, P1.GetAvailableManaCount());
      }
    }
  }
}