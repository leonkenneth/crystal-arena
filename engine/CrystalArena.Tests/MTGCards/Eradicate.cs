namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Quash
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void CounterAndRemoveFromPlay()
      {
        Hand(P1, "Duress", "Duress");
        Hand(P2, "Quash", "Island", "Island", "Island");
        BreakZone(P1, "Duress");
        MainDeck(P1, "Duress");
        
        Battlefield(P1, "Swamp");
        Battlefield(P2, "Island", "Island", "Island", "Island");

        RunGame(1);

        Equal(4, P1.RemovedFromPlay.Count(x => x.Name.Equals("Duress")));
      }
    }
  }
  
  public class Eradicate
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void RemoveFromPlay()
      {
        Hand(P1, "Eradicate");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp");
        Battlefield(P2, "Llanowar Behemoth");
        BreakZone(P2, "Llanowar Behemoth");
        MainDeck(P2, "Llanowar Behemoth");
        Hand(P2, "Llanowar Behemoth");

        RunGame(1);

        Equal(4, P2.RemovedFromPlay.Count(x => x.Name.Equals("Llanowar Behemoth")));
      }
    }
  }
}