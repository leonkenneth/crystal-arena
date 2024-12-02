namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Disorder
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DoNotCastDisorderWhenNoEffect()
      {
        var disorder = C("Disorder");

        Hand(P1, disorder);
        Battlefield(P1, "Mountain", "Mountain");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

        RunGame(2);

        Equal(Zone.Hand, C(disorder).Zone);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void DealDamageToWhiteForwardsAndTheirControllers()
      {
        var disorder = C("Disorder");

        Hand(P1, disorder);
        Battlefield(P1, "Grizzly Bears");
        Battlefield(P2, "Light Knight");

        Exec(
          At(Step.FirstMain)
            .Cast(disorder)
            .Verify(() =>
              {
                Equal(0, P1.BreakZone.Forwards.Count());
                Equal(20, P1.Life);
                Equal(1, P2.BreakZone.Count());
                Equal(18, P2.Life);
              })
          );
      }
    }
  }
}