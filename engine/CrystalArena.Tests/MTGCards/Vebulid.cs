namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Vebulid
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void AttackAndDestroy()
      {
        var vebuild = C("Vebulid");

        Hand(P1, vebuild);

        Exec(
          At(Step.FirstMain)
            .Cast(vebuild),
          At(Step.DeclareAttackers, turn: 3)
            .DeclareAttackers(vebuild),
          At(Step.SecondMain, turn: 3)
            .Verify(() =>
              {
                Equal(Zone.BreakZone, C(vebuild).Zone);
                Equal(18, P2.Life);
              })
          );
      }
    }
  }
}