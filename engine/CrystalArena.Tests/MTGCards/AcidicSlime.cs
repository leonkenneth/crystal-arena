namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AcidicSlime
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyBackup()
      {
        var slime = C("Acidic Slime");
        var forest = C("Forest");

        Hand(P1, slime);
        Battlefield(P2, forest);

        Exec(
          At(Step.FirstMain)
            .Cast(slime)
            .Target(forest)
            .Verify(() => Equal(Zone.BreakZone, C(forest).Zone))
          );
      }
    }
  }
}