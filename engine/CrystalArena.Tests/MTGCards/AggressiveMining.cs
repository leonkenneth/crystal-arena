namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AggressiveMining
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void YouCannotPlayBackups()
      {
        Battlefield(P1, "Aggressive Mining");
        Hand(P1, "Grizzly Bears", "Forest");

        RunGame(1);

        Equal(2, P1.Hand.Count);
      }
    }
  }
}
