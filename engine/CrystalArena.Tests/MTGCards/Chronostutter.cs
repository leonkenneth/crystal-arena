namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Chronostutter
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void PutBearSecondFromTheTop()
      {
        var bear = C("Grizzly Bears");
        MainDeck(P1, "Juggernaut", "Juggernaut");
        Battlefield(P1, bear);

        P2.Life = 2;
        Battlefield(P2, "Island", "Island", "Island", "Island", "Island", "Island");
        Hand(P2, "Chronostutter");

        RunGame(1);

        Equal(C(bear), P1.MainDeck.Skip(1).First());
      }
    }
  }
}