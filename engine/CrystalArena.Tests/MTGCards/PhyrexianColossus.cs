namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PhyrexianColossus
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void CanBeBlockedBy3()
      {
        Battlefield(P1, "Phyrexian Colossus");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        P2.Life = 8;
        
        RunGame(1);

        Equal(0, P2.Battlefield.Count);
      }

      [Fact (Skip = "Old card")]
      public void HasToBeUntappedByPaying8Life()
      {
        Battlefield(P1, C("Phyrexian Colossus").Tap());
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

        P2.Life = 8;
        
        RunGame(1);

        Equal(12, P1.Life);
        Equal(0, P2.Life);
      }
    }
  }
}