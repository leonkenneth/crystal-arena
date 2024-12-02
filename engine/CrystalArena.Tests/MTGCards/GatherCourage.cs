namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GatherCourage
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void CastWithConvoke()
      {
        Hand(P1, "Gather Courage");
        Battlefield(P1, "Sunblade Elf", "Welkin Tern");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
      }      
    }
  }
}