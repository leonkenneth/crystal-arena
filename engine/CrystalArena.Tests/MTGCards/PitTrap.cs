namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PitTrap
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyForce()
      {
        var force = C("Verdant Force");

        Battlefield(P1, force);
        Battlefield(P2, "Pit Trap", "Swamp", "Swamp");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(force),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.BreakZone, C(force).Zone))
          );
      }
    }
  }
}