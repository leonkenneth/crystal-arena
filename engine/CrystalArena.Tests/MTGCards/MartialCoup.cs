namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class MartialCoup
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void JustCreateTokens()
      {
        var coup = C("Martial Coup");
        var dragon = C("Shivan Dragon");

        Hand(P1, coup);
        Battlefield(P1, dragon, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(5, P1.Battlefield.Forwards.Count());
        Equal(Zone.Battlefield, C(dragon).Zone);
      }

      [Fact (Skip = "Old card")]
      public void DestroyOtherForwards()
      {
        var coup = C("Martial Coup");

        Hand(P1, coup);
        Battlefield(P1, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(7, P1.Battlefield.Forwards.Count());
        Equal(0, P2.Battlefield.Forwards.Count());
      }
    }


    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void JustCreateTokens()
      {
        var coup = C("Martial Coup");

        Hand(P1, coup);
        Battlefield(P1, "Grizzly Bears");

        Exec(
          At(Step.FirstMain)
            .Cast(coup, x: 4)
            .Verify(() => Equal(5, P1.Battlefield.Forwards.Count()))
          );
      }

      [Fact (Skip = "Old card")]
      public void DestroyOtherForwards()
      {
        var coup = C("Martial Coup");

        Hand(P1, coup);
        Battlefield(P1, "Grizzly Bears");
        Battlefield(P2, "Grizzly Bears");

        Exec(
          At(Step.FirstMain)
            .Cast(coup, x: 5)
            .Verify(() =>
              {
                Equal(5, P1.Battlefield.Forwards.Count());
                Equal(0, P2.Battlefield.Forwards.Count());
              })
          );
      }
    }
  }
}