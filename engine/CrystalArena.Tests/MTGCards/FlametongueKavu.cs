namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class FlametongueKavu
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void BugDoNotPlayKavuIfOnlyForward()
      {
        var kavu = C("Flametongue Kavu");

        Hand(P1, kavu);
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain");

        RunGame(1);
        Equal(Zone.Hand, C(kavu).Zone);
      }

      [Fact (Skip = "Old card")]
      public void BugGameCrashesWhenPlayingKavu()
      {
        Battlefield(P1, "Forest", "Mountain", "Llanowar Elves", "Llanowar Elves", "Forest");
        var kavu = C("Flametongue Kavu");
        Hand(P1, kavu);

        var bear = C("Grizzly Bears");
        Battlefield(P2, "Forest", "Mountain", C("Llanowar Elves").Tap(), "Llanowar Elves", bear);

        RunGame(maxTurnCount: 2);

        Equal(Zone.Battlefield, C(kavu).Zone);
        Equal(Zone.BreakZone, C(bear).Zone);
      }

      [Fact (Skip = "Old card")]
      public void KavuEatsTheElephant()
      {
        var kavu = C("Flametongue Kavu");
        Hand(P1, kavu);
        Battlefield(P1, C("Forest"), C("Forest"), C("Forest"), C("Mountain"));
        Battlefield(P2, C("Trained Armodon"), C("Shivan Dragon"));

        RunGame(maxTurnCount: 1);

        Equal(Zone.Battlefield, C(kavu).Zone);
        Equal(1, P2.Battlefield.Count());
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void Deals4DamageToTargetForward()
      {
        var kavu = C("Flametongue Kavu");
        var bear = C("Grizzly Bears");

        Hand(P1, kavu);
        Battlefield(P2, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(kavu)
            .Target(bear)
            .Verify(() =>
              {
                Equal(1, P2.BreakZone.Count());
                Equal(1, P1.Battlefield.Count());
              })
          );
      }

      [Fact (Skip = "Old card")]
      public void KavuTargetIsDestroyed()
      {
        var kavu = C("Flametongue Kavu");
        var bear = C("Grizzly Bears");
        var shock = C("Shock");

        Hand(P1, kavu);
        Hand(P2, shock);
        Battlefield(P2, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(kavu)
            .Target(bear)
            .Cast(shock, target: bear)
            .Verify(() =>
              {
                Equal(2, P2.BreakZone.Count());
                Equal(0, C(bear).Damage);
                Equal(1, P1.Battlefield.Count());
              })
          );
      }
    }
  }
}