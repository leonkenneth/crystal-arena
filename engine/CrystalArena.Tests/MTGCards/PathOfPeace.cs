namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PathOfPeace
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyForce()
      {
        var force = C("Verdant Force");

        Hand(P1, "Path of Peace");
        Battlefield(P1, "Plains", "Forest", "Forest", "Forest");
        Battlefield(P2, force);

        RunGame(1);

        Equal(Zone.BreakZone, C(force).Zone);
        Equal(24, P2.Life);
      }
    }
  }
}