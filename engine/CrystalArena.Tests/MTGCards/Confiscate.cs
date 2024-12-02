namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Confiscate
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void ConfiscateForward()
      {
        var bear = C("Grizzly Bears");
        var confiscate = C("Confiscate");
        var disenchant = C("Disenchant");

        Hand(P1, confiscate);
        Hand(P2, disenchant);
        Battlefield(P2, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(confiscate, target: bear),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(P1, C(bear).Controller);
                True(C(bear).HasSummoningSickness);
              }),
          At(Step.EndOfTurn)
            .Cast(disenchant, confiscate)
            .Verify(() => Equal(P2, C(bear).Controller))
          );
      }

      [Fact (Skip = "Old card")]
      public void ConfiscateUrzasArmor()
      {
        var armor = C("Urza's armor");
        var confiscate = C("Confiscate");
        var shock = C("Shock");

        Hand(P1, confiscate);
        Hand(P2, shock);
        Battlefield(P2, armor);

        Exec(
          At(Step.FirstMain)
            .Cast(confiscate, target: armor),
          At(Step.SecondMain)
            .Cast(shock, target: P1)
            .Verify(() => Equal(19, P1.Life))          
        );
      }

      [Fact (Skip = "Old card")]
      public void ConfiscateConfiscate()
      {
        var confiscate1 = C("Confiscate");
        var confiscate2 = C("Confiscate");
        var bear1 = C("Grizzly Bears");

        Hand(P1, confiscate1);
        Hand(P2, confiscate2);
        
        Battlefield(P2, bear1);

        Exec(
          At(Step.FirstMain)
            .Cast(confiscate1, target: bear1),
          At(Step.FirstMain, turn: 2)
            .Cast(confiscate2, target: confiscate1)
            .Verify(() =>
              {
                Equal(P1, C(bear1).Controller);
                True(P1.Battlefield.Any(x => x == C(confiscate2)));
              })
        );
      }

      [Fact (Skip = "Old card")]
      public void EchoNeedsToBePaid()
      {
        var raptor = C("Shivan Raptor");
        var confiscate = C("Confiscate");        

        Hand(P1, confiscate);        
        Battlefield(P2, raptor);

        Exec(
          At(Step.FirstMain)
            .Cast(confiscate, target: raptor),
          At(Step.FirstMain, turn: 3)
            .Verify(() =>
              {
                Equal(Zone.BreakZone, C(raptor).Zone);
              }));
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyWithLyrist()
      {
        var confiscate = C("Confiscate");
        var armodon = C("Trained Armodon");
        
        Hand(P1, confiscate);        
        Battlefield(P2, "Elvish Lyrist", armodon, "Forest");

        Exec(
          At(Step.FirstMain)
            .Cast(confiscate, target: armodon),
          At(Step.FirstMain, turn: 2)
            .Verify(() =>
              {
                Equal(Zone.BreakZone, C(confiscate).Zone);
                Equal(P2, C(armodon).Controller);
              })

        );

      }
    }
  }
}