namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TickingGnomes
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void SacGnomesToKillOpponent()
      {
        Battlefield(P1, "Ticking Gnomes", "Forest", "Forest", "Forest");
        P2.Life = 4;

        RunGame(2);

        Equal(0, P2.Life);
      }
    }
  }
}