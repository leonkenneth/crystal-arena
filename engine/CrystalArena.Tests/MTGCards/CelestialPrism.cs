namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CelestialPrism
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void CastBear()
      {
        var bear = C("Grizzly Bears");

        Hand(P1, bear);
        Battlefield(P1, "Plains", "Plains", "Plains", "Celestial Prism");

        RunGame(1);

        Equal(Zone.Battlefield, C(bear).Zone);
      }
    }
  }
}