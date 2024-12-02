namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ViashinoRunner
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void CannotBeBlockedBy1Forward()
      {
        Battlefield(P1, "Viashino Runner");
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 3;

        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact (Skip = "Old card")]
      public void CanBeBlockedBy2Forwards()
      {
        Battlefield(P1, "Viashino Runner");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

        P2.Life = 3;

        RunGame(1);

        Equal(3, P2.Life);
      }
    }
  }
}