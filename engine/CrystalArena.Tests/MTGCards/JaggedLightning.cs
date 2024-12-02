namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class JaggedLightning
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void Kill2Forwards()
      {
        var armodon = C("Trained Armodon");
        var bear = C("Grizzly Bears");

        Hand(P1, "Jagged Lightning");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, armodon, bear);

        RunGame(1);

        Equal(Zone.BreakZone, C(armodon).Zone);
        Equal(Zone.BreakZone, C(bear).Zone);
      }

      [Fact (Skip = "Old card")]
      public void CannotKillOnlyOne()
      {
        var armodon = C("Trained Armodon");

        Hand(P1, "Jagged Lightning");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, armodon);

        RunGame(1);

        Equal(Zone.Battlefield, C(armodon).Zone);
      }

      [Fact (Skip = "Old card")]
      public void DoNotKillYours()
      {
        var armodon = C("Trained Armodon");
        var bear = C("Grizzly Bears");

        Hand(P1, "Jagged Lightning");
        Battlefield(P1, bear, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, armodon);

        RunGame(1);

        Equal(Zone.Battlefield, C(armodon).Zone);
        Equal(Zone.Battlefield, C(bear).Zone);
      }
    }
  }
}