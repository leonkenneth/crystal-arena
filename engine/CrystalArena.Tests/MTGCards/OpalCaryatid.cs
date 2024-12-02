namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class OpalCaryatid
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void BothBecomeForwards()
      {
        var bear = C("Grizzly Bears");
        var opal1 = C("Opal Caryatid");
        var opal2 = C("Opal Caryatid");

        Hand(P1, bear);
        Battlefield(P2, opal1, opal2);

        Exec(
          At(Step.FirstMain)
            .Cast(bear)
            .Verify(() =>
              {
                True(C(opal1).Is().Forward);
                True(C(opal2).Is().Forward);
              })
          );
      }
    }
  }
}