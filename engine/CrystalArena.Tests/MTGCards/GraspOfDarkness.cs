namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class GraspOfDarkness
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void CardWithZeroToughnessGoesToBreakZone()
      {
        var elves = C("Llanowar Elves");
        var grasp = C("Grasp of Darkness");

        Battlefield(P2, elves);
        Hand(P1, grasp);

        Exec(
          At(Step.FirstMain)
            .Cast(grasp, target: elves)
            .Verify(() =>
              Equal(0, P2.Battlefield.Count()))
          );
      }
    }
  }
}