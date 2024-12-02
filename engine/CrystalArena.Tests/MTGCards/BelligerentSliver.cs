namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BelligerentSliver
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void CannotBeBlockedBy1Forward()
      {
        Battlefield(P1, "Belligerent Sliver", "Leeching Sliver");
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 5;

        RunGame(1);

        Equal(0, P2.BreakZone.Count);
        Equal(0, P1.BreakZone.Count);
        Equal(0, P2.Life);
      }

      [Fact (Skip = "Old card")]
      public void CanBeBlockedBy2Forwards()
      {
        Battlefield(P1, "Belligerent Sliver", "Belligerent Sliver");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

        P2.Life = 4;

        RunGame(1);

        Equal(2, P2.Life);
      }
    }
  }
}
