namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class NissaWorldwaker
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void SearchForBackups()
      {
        var nissa = C("Nissa, Worldwaker");

        Battlefield(P1, nissa.AddCounters(9, CounterType.Loyality));
        MainDeck(P1, "Grizzly Bears", "Island", "Island", "Forest", "Grizzly Bears", "Forest", "Forest");

        P2.Life = 20;

        RunGame(3);

        Equal(0, P2.Life);
      }
    }
  }
}