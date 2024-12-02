namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class HeadlongRush
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void GiveWhaleFirstStrike()
      {
        Hand(P1, "Headlong Rush");
        Battlefield(P1, "Great Whale", "Mountain", "Mountain");
        Battlefield(P2, "Great Whale");

        RunGame(1);

        Equal(1, P1.Battlefield.Forwards.Count());
        Equal(0, P2.Battlefield.Forwards.Count());
      }
    }
  }
}