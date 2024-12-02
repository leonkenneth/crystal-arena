namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SwordsToPlowshares
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void RemoveFromPlayForce()
      {
        var force = C("Verdant Force");

        Battlefield(P1, "Plains");
        Hand(P1, "Swords to Plowshares");
        Battlefield(P2, force);

        RunGame(maxTurnCount: 2);

        Equal(Zone.RemovedFromPlay, C(force).Zone);
        Equal(27, P2.Life);
      }
    }
  }
}