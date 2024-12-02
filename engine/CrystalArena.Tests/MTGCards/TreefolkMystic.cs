namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TreefolkMystic
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyAttackersAuras()
      {
        var bears = C("Grizzly Bears");
        Battlefield(P1, bears.IsEnchantedWith("Gaea's Embrace"));
        Battlefield(P2, "Treefolk Mystic");

        P2.Life = 5;

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bears),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.BreakZone, C(bears).Zone))
          );
      }

      [Fact (Skip = "Old card")]
      public void DestroyBlockersAuras()
      {
        var bears = C("Grizzly Bears");
        var mystic = C("Treefolk Mystic");
        
        Battlefield(P1, mystic);
        Battlefield(P2, bears.IsEnchantedWith("Gaea's Embrace"));
        
        P2.Life = 2;

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(mystic),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.BreakZone, C(bears).Zone))
          );
      }
    }
  }
}