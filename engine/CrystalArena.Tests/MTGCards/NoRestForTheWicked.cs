namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class NoRestForTheWicked
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void Return1Forward()
      {
        var force1 = C("Verdant Force");
        var force2 = C("Verdant Force");
        var blade1 = C("Doom blade");
        var blade2 = C("Doom blade");
        var rest = C("No Rest for the wicked");

        Battlefield(P2, rest, force1, force2);
        Hand(P1, blade1, blade2);

        Exec(
          At(Step.FirstMain, 1)
            .Cast(blade1, target: force1),
          At(Step.FirstMain, 2)
            .Cast(blade2, target: force2),
          At(Step.SecondMain, 2)
            .Activate(rest)
            .Verify(() =>
              {
                Equal(Zone.BreakZone, C(force1).Zone);
                Equal(Zone.Hand, C(force2).Zone);
                Equal(Zone.BreakZone, C(rest).Zone);
              }));          
      }
    }
  }
}