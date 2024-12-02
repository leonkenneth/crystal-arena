namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CreepingTarPit
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void Destroy()
      {
        var burst = C("Burst Lightning");
        var pit = C("Creeping Tar Pit");

        Hand(P2, burst);
        Battlefield(P1, pit);

        Exec(
          At(Step.FirstMain)
            .Activate(pit, abilityIndex: 1),
          At(Step.DeclareAttackers)
            .DeclareAttackers(pit),
          At(Step.DeclareBlocker)
            .Cast(burst, target: pit),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.BreakZone, C(pit).Zone))
          );
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact (Skip = "Old card")]
      public void Destroy()
      {
        var burst = C("Burst Lightning");
        var pit = C("Creeping Tar Pit");

        Hand(P2, burst);
        Battlefield(P2, "Mountain", "Mountain");
        Battlefield(P1, pit);

        Exec(
          At(Step.FirstMain)
            .Activate(pit, abilityIndex: 1),
          At(Step.DeclareAttackers)
            .DeclareAttackers(pit),
          At(Step.SecondMain)
            .Verify(() => { Equal(Zone.BreakZone, C(pit).Zone); })
          );
      }
    }
  }
}