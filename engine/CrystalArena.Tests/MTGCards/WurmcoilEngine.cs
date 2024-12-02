namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class WurmcoilEngine
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void DeathtouchAndLifelinkAnd2Tokens()
      {
        var engine = C("Wurmcoil Engine");
        var force1 = C("Verdant Force");
        var force2 = C("Verdant Force");

        Battlefield(P1, engine);
        Battlefield(P2, force1, force2);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(engine),
          At(Step.DeclareBlocker)
            .DeclareBlockers(engine, force1, engine, force2),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.BreakZone, C(force1).Zone);
                Equal(Zone.BreakZone, C(force2).Zone);
                Equal(2, P1.Battlefield.Count());
                Equal(26, P1.Life);
              })
          );
      }

      [Fact (Skip = "Old card")]
      public void LifelinkDamageToPlayer()
      {
        var engine = C("Wurmcoil Engine");
        Battlefield(P1, engine);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(engine),
          At(Step.SecondMain)
            .Verify(() => Equal(26, P1.Life)));
      }
    }
  }
}