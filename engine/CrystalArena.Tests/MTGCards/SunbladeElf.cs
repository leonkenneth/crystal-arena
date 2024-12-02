namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SunbladeElf
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void AttackFor6()
      {
        Battlefield(P1, "Sunblade Elf", "Grizzly Bears", "Forest", "Forest", "Plains", "Forest", "Forest");
        
        P2.Life = 6;
        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}