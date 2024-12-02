namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Phytotitan
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void PhythotianComesBack()
      {
        var phytotitan = C("Phytotitan");
        
        Battlefield(P1, "Thran War Machine", "Mountain", "Mountain", "Mountain", "Mountain");                        
        Battlefield(P2, phytotitan);        

        RunGame(2);
        
        Equal(Zone.Battlefield, C(phytotitan).Zone);
        True(C(phytotitan).IsTapped);
        Equal(20, P1.Life);
        Equal(20, P2.Life);    
      }
    }
  }
}