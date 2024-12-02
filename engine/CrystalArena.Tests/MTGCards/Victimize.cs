namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Victimize
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DoNotCastWithOnlyOneForwardInBreakZone()
      {
        var force = C("Verdant Force");
        var bear = C("Grizzly bears");

        Hand(P1, "Victimize");
        BreakZone(P1, force);
        Battlefield(P1, bear, "Swamp", "Forest", "Forest");

        RunGame(1);

        Equal(Zone.BreakZone, C(force).Zone);
      }

      [Fact (Skip = "Old card")]
      public void BringBack2Forwards()
      {
        var force = C("Verdant Force");
        var dragon = C("Shivan Dragon");
        var bear = C("Grizzly bears");

        Hand(P1, "Victimize");
        BreakZone(P1, force, dragon);
        Battlefield(P1, bear, "Swamp", "Forest", "Forest");

        RunGame(1);

        Equal(Zone.Battlefield, C(dragon).Zone);
        Equal(Zone.Battlefield, C(force).Zone);
        Equal(Zone.BreakZone, C(bear).Zone);
      }
    }
  }
}