namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CaveTiger
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void Gets11()
      {
        var bear = C("Grizzly Bears");
        var tiger = C("Cave Tiger");


        Battlefield(P1, tiger);
        Battlefield(P2, bear);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(tiger),
          At(Step.DeclareBlocker)
            .DeclareBlockers(tiger, bear),
          At(Step.SecondMain)
            .Verify(() => Equal(3, C(tiger).Power))
          );
      }
    }
  }
}