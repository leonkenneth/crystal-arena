namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class IridescentDrake
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void EnchantWithRancor()
      {
        var drake = C("Iridescent Drake");
        Hand(P1, drake);
        Battlefield(P1, "Island", "Island", "Forest", "Forest");
        BreakZone(P2, "Rancor");

        RunGame(1);

        Equal(Zone.Battlefield, C(drake).Zone);
        Equal(4, C(drake).Power);
      }
    }
  }
}