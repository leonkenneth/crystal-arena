namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PatternOfRebirth
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void SearchForDragon()
      {
        var dragon = C("Shivan Dragon");
        
        Battlefield(P1, C("Grizzly Bears").IsEnchantedWith("Pattern of Rebirth"));        
        MainDeck(P1, dragon);

        Hand(P2, "Shock");
        Battlefield(P2, "Mountain");
        P2.Life = 2;

        RunGame(1);

        Equal(Zone.Battlefield, C(dragon).Zone);
      }
    }
  }
}