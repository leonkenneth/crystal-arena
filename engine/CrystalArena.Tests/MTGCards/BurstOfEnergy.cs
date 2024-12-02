namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BurstOfEnergy
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void UntapBalothAndBlock()
      {
        var armodon = C("Trained Armodon");
        Battlefield(P1, armodon);
        Battlefield(P2, C("Ravenous Baloth").Tap(), "Plains");
        Hand(P2, "Burst of Energy");
        P2.Life = 3;

        RunGame(1);

        Equal(Zone.BreakZone, C(armodon).Zone);
      }
    }
  }
}