namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BodySnatcher
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void RemoveFromPlayBodySnatcher()
      {
        var snatcher = C("Body Snatcher");

        Hand(P1, snatcher);

        Exec(
          At(Step.FirstMain)
            .Cast(snatcher),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.RemovedFromPlay, C(snatcher).Zone))
        );
      }

      [Fact (Skip = "Old card")]
      public void PutForceToBattlefield()
      {
        var snatcher = C("Body Snatcher");
        var force = C("Verdant Force");
        var shock = C("Shock");

        Hand(P1, snatcher, force);
        Hand(P2, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(snatcher),
          At(Step.DeclareAttackers)
            .Cast(shock, target: snatcher)
            .Target(force),            
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.RemovedFromPlay, C(snatcher).Zone);
                Equal(Zone.Battlefield, C(force).Zone);
              })
        );
      }
    }
  }
}