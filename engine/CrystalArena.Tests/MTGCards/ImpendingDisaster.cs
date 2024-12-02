namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ImpendingDisaster
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyAllBackups()
      {
        Battlefield(P1, "Forest", "Forest", "Forest", "Impending Disaster");
        Battlefield(P2, "Mountain", "Forest", "Forest", "Mountain");

        RunGame(1);

        Equal(0, P1.Battlefield.Count);
        Equal(0, P2.Battlefield.Count);
      }
    }
  }
}