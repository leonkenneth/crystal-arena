namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TreetopVillage
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void AttackToKill()
      {
        Battlefield(P1, "Treetop Village", "Forest", "Forest");
        Battlefield(P2, "Llanowar Elves");

        P2.Life = 2;
        RunGame(1);

        True(P2.Life <= 0);
      }
    }
  }
}