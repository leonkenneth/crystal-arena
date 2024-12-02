namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DarkestHour
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void BearsBecomeBlack()
      {
        var hour = C("Darkest Hour");
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");

        Hand(P1, hour);
        Battlefield(P1, bear1);
        Battlefield(P2, bear2);

        Exec(
          At(Step.FirstMain)
            .Cast(hour)
            .Verify(() =>
              {
                True(C(bear1).HasColor(CardColor.Dark));
                True(C(bear2).HasColor(CardColor.Dark));

                False(C(bear1).HasColor(CardColor.Wind));
                False(C(bear2).HasColor(CardColor.Wind));
              })
          );
      }
    }
  }
}