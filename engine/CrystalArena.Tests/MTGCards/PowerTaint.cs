namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PowerTaint
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void EnchantWorship()
      {
        Hand(P1, "Power Taint");
        Battlefield(P1, "Island", "Island");
        Battlefield(P2, "Worship");

        P2.Life = 2;
        RunGame(2);

        Equal(0, P2.Life);
      }
    }
  }
}