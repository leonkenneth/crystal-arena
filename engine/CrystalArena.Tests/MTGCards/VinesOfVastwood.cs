namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class VinesOfVastwood
  {
    public class Ai : PredefinedAiScenario
    {
      [Fact (Skip = "Old card")]
      public void VinesOfVastwoodRepellsShock()
      {
        var shock = C("Shock");
        var bear = C("Grizzly bears");
        var vines = C("Vines of Vastwood");

        Battlefield(P2, bear, "Forest", "Forest");

        Hand(P1, shock);
        Hand(P2, vines);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, bear)
            .Verify(
              () =>
                {
                  Equal(Zone.Battlefield, C(bear).Zone);
                  Equal(Zone.BreakZone, C(vines).Zone);
                }
            ));
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void WithKicker()
      {
        var vines = C("Vines of Vastwood");
        var bear = C("Grizzly Bears");

        Battlefield(P1, bear);
        Hand(P1, vines);

        Exec(
          At(Step.FirstMain)
            .Cast(vines, bear, index: 1)
            .Verify(() =>
              {
                True(C(bear).Has().Hexproof);
                Equal(6, C(bear).Power);
                Equal(6, C(bear).Toughness);
              })
          );
      }

      [Fact (Skip = "Old card")]
      public void WithoutKicker()
      {
        var vines = C("Vines of Vastwood");
        var bear = C("Grizzly Bears");

        Battlefield(P1, bear);
        Hand(P1, vines);

        Exec(
          At(Step.FirstMain)
            .Cast(vines, bear)
            .Verify(() => True(C(bear).Has().Hexproof)
            ));
      }
    }
  }
}