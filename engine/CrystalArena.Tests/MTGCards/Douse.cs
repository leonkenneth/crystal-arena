namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Douse
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact (Skip = "Old card")]
      public void CounterDragon()
      {
        var dragon = C("Shivan Dragon");
        Hand(P1, dragon);
        Battlefield(P2, "Island", "Island", "Douse");

        Exec(
          At(Step.FirstMain)
            .Cast(dragon)
            .Verify(() => Equal(Zone.BreakZone, C(dragon).Zone))
          );
      }
    }
  }
}