namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Whetstone
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void WinTheGameByMillingOpponent()
      {
        RemoveFromPlayMainDeck(P2);
        MainDeck(P2, "Swamp", "Swamp");

        Battlefield(P1, "Whetstone", "Swamp", "Swamp" ,"Swamp");

        RunGame(2);

        True(P2.HasLost);
      }
    }
  }
}